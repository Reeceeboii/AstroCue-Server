namespace AstroCue.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Astronomy;
    using Data;
    using Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models.Email.Reports;
    using Models.Misc;

    /// <summary>
    /// Service class for handling the reports
    /// </summary>
    public class ReportService : IReportService
    {
        /// <summary>
        /// Instance of <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Instance of <see cref="EmailService"/>
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// Instance of <see cref="IWeatherForecastService"/>
        /// </summary>
        private readonly IWeatherForecastService _weatherForecastService;

        /// <summary>
        /// Initialises a new instance of the <see cref="ReportService"/> class
        /// </summary>
        /// <param name="context">Instance of <see cref="ApplicationDbContext"/></param>
        /// <param name="emailService">Instance of <see cref="EmailService"/></param>
        /// <param name="weatherForecastService">Instance of <see cref="IWeatherForecastService"/></param>
        public ReportService(
            ApplicationDbContext context,
            IEmailService emailService,
            IWeatherForecastService weatherForecastService)
        {
            this._context = context;
            this._emailService = emailService;
            this._weatherForecastService = weatherForecastService;
        }

        public void GenerateReports()
        {
            IList<AstroCueUser> users = this._context.AstroCueUsers.ToList();

            foreach (AstroCueUser user in users)
            {
                IList<Observation> usersObservations = this._context.Observations
                    .Include(o => o.ObservationLocation)
                    .Include(o => o.AstronomicalObject)
                    .Where(o => o.ObservationLocation.AstroCueUserId == user.Id)
                    .ToList();

                // map observation locations to all of the observations they form a part of
                IDictionary<ObservationLocation, IList<Observation>> locToObsMap =
                    new Dictionary<ObservationLocation, IList<Observation>>();

                foreach (Observation observation in usersObservations)
                {
                    if (locToObsMap.ContainsKey(observation.ObservationLocation))
                    {
                        locToObsMap[observation.ObservationLocation].Add(observation);
                    }
                    else
                    {
                        locToObsMap[observation.ObservationLocation] = new List<Observation>
                        {
                            observation
                        };
                    }
                }

                List<LocationReport> reportList = new();

                foreach (KeyValuePair<ObservationLocation, IList<Observation>> entry in locToObsMap)
                {
                    FourDayForecastReport forecastForLocation =
                        this._weatherForecastService.GetForecastNextFourDays(
                            entry.Key.Longitude,
                            entry.Key.Latitude).Result;

                    LocationReport report = new()
                    {
                        Name = entry.Key.Name,
                        Longitude = entry.Key.Longitude,
                        Latitude = entry.Key.Latitude,
                        StaticMapImageName = $"location_{entry.Key.Id}.png",
                        Sunrise = forecastForLocation.Sunrise.ToString("mm:ss tt"),
                        Sunset = forecastForLocation.Sunset.ToString("mm:ss tt"),
                        Objects = new List<SingleObjectReport>()
                    };

                    DateTime timeOfReportGenUtc = DateTime.UtcNow;

                    // get round hour for start of weather forecast analysis
                    timeOfReportGenUtc = timeOfReportGenUtc.AddMinutes(-timeOfReportGenUtc.Minute);
                    timeOfReportGenUtc = timeOfReportGenUtc.AddSeconds(-timeOfReportGenUtc.Second);

                    HourlyForecast best = null;
                    DateTime timeOfBest = DateTime.Now;
                    float bestIndex = 0f;

                    DateTime endOfWindow = timeOfReportGenUtc.AddHours(forecastForLocation.Forecasts.Count);
                    for (DateTime i = timeOfReportGenUtc; i <= endOfWindow; i = i.AddHours(1))
                    {
                        // if iterated hour does not fall into nighttime
                        if (!(i.Hour > forecastForLocation.Sunset.Hour || i.Hour < forecastForLocation.Sunrise.Hour))
                        {
                            continue;
                        }

                        // construct a new EqTimeRecord instance for easy indexing into forecast list
                        EqTimeRecord eq = new()
                        {
                            Day = i.Day,
                            Hour = i.Hour
                        };

                        // if there's no weather forecast instance available (there should be, this is just a sanity check)
                        if (!forecastForLocation.Forecasts.TryGetValue(eq, out HourlyForecast forecastInstance))
                        {
                            continue;
                        }

                        if (best == null)
                        {
                            best = forecastInstance;
                            bestIndex = WeatherForecastRating.CalculateIndex(forecastInstance);
                            timeOfBest = i;
                        }
                        else
                        {
                            float index = WeatherForecastRating.CalculateIndex(forecastInstance);

                            if (!(index < bestIndex))
                            {
                                continue;
                            }

                            best = forecastInstance;
                            bestIndex = index;
                            timeOfBest = i;
                        }
                    }

                    if (best == null)
                    {
                        throw new Exception("Best observation time is null");
                    }

                    report.BestTimeToObserve = $"{timeOfBest:dd/mm/yy} @ {timeOfBest:t} (UTC)";
                    report.CloudCoveragePercent = best.CloudCoveragePercent;
                    report.TemperatureCelcius = best.TemperatureCelcius;
                    report.HumidityPercent = best.HumidityPercent;
                    report.WindSpeedMetersPerSec = best.WindSpeedMetersPerSec;
                    report.ProbabilityOfPrecipitation = best.ProbabilityOfPrecipitation;
                    report.Description = best.Description;

                    // apply any warnings
                    if (best.CloudCoveragePercent > 50)
                    {
                        report.WeatherWarnings = "Cloud coverage >50%, observations may be difficult";
                    }
                }

            }
        }
    }
}
