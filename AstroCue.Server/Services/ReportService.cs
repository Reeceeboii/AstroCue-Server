namespace AstroCue.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Astronomy;
    using AutoMapper;
    using Data;
    using Entities;
    using Entities.Owned;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models.API.Outbound;
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
        /// Instance of <see cref="IMappingService"/>
        /// </summary>
        private readonly IMappingService _mappingService;

        /// <summary>
        /// Instance of <see cref="IMapper"/>
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="ReportService"/> class
        /// </summary>
        /// <param name="context">Instance of <see cref="ApplicationDbContext"/></param>
        /// <param name="emailService">Instance of <see cref="EmailService"/></param>
        /// <param name="weatherForecastService">Instance of <see cref="IWeatherForecastService"/></param>
        /// <param name="mappingService">Instance of <see cref="IMappingService"/></param>
        /// <param name="mapper">Instance of <see cref="IMapper"/></param>
        public ReportService(
            ApplicationDbContext context,
            IEmailService emailService,
            IWeatherForecastService weatherForecastService,
            IMappingService mappingService,
            IMapper mapper)
        {
            this._context = context;
            this._emailService = emailService;
            this._weatherForecastService = weatherForecastService;
            this._mappingService = mappingService;
            this._mapper = mapper;
        }

        /// <summary>
        /// Main crux of the AstroCue platform - generates astronomical observation reports,
        /// persists them to the database and emails them to users
        /// </summary>
        /// <param name="userIdForceGenerate">An ID given if the report generation process was kicked off manually
        /// by a user. Defaulted to 0 for all other cases</param>
        /// <returns><see cref="Task"/></returns>
        /// <exception cref="Exception">If the report generation process was started manually but the user
        /// who invoked it has no observations set up on their account</exception>
        public async Task GenerateReports(int userIdForceGenerate = 0)
        {
            IList<AstroCueUser> users;

            /*
             * If a non-zero user ID is provided, reports will be generated for a single
             * user at the instant the method was invoked, else, standard set time reports
             * are generated for every user that has observations set up
             */
            if (userIdForceGenerate > 0)
            {
                users = this._context.AstroCueUsers
                    .Where(u => u.Id == userIdForceGenerate)
                    .ToList();
            }
            else
            {
                users = this._context.AstroCueUsers.ToList();
            }
            
            foreach (AstroCueUser user in users)
            {
                IList<Observation> usersObservations = this._context.Observations
                    .Include(o => o.ObservationLocation)
                    .Include(o => o.AstronomicalObject)
                    .Where(o => o.ObservationLocation.AstroCueUserId == user.Id)
                    .ToList();

                if (userIdForceGenerate > 0)
                {
                    if (usersObservations.Count == 0)
                    {
                        throw new Exception("You need to have at least one observation set up before reports can be created");
                    }
                }

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
                Dictionary<string, byte[]> staticMaps = new();

                // for every location that the user has created
                foreach (KeyValuePair<ObservationLocation, IList<Observation>> entry in locToObsMap)
                {
                    // fetch weather forecast for location for next n days
                    FourDayForecastReport forecastForLocation =
                        this._weatherForecastService.GetForecastNextFourDays(
                            entry.Key.Longitude,
                            entry.Key.Latitude).Result;

                    // fetch static map image for location and add to dictionary against image name as key
                    string staticMapImageName = $"location_{entry.Key.Id}.png";
                    byte[] locStaticMap = this._mappingService.GetStaticMapImageAsync(entry.Key.Longitude, entry.Key.Latitude).Result;
                    staticMaps.Add(staticMapImageName, locStaticMap);

                    LocationReport report = new()
                    {
                        Name = entry.Key.Name,
                        Longitude = entry.Key.Longitude,
                        Latitude = entry.Key.Latitude,
                        StaticMapImageName = staticMapImageName,
                        Sunrise = forecastForLocation.Sunrise.ToString("t"),
                        Sunset = forecastForLocation.Sunset.ToString("t"),
                        Objects = new List<List<SingleObjectReport>>()
                    };

                    DateTime timeOfReportGenUtc = DateTime.UtcNow;

                    // get round hour for start of weather forecast analysis
                    timeOfReportGenUtc = timeOfReportGenUtc.AddMinutes(-timeOfReportGenUtc.Minute);
                    timeOfReportGenUtc = timeOfReportGenUtc.AddSeconds(-timeOfReportGenUtc.Second);

                    HourlyForecast best = null;
                    DateTime timeOfBest = DateTime.Now.ToUniversalTime();
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

                    // apply details to report
                    report.BestTimeToObserve = $"{timeOfBest:M} @ {timeOfBest:t} (UTC)";
                    report.CloudCoveragePercent = best.CloudCoveragePercent;
                    report.TemperatureCelcius = best.TemperatureCelcius;
                    report.HumidityPercent = best.HumidityPercent;
                    report.WindSpeedMetersPerSec = best.WindSpeedMetersPerSec;
                    report.ProbabilityOfPrecipitation = best.ProbabilityOfPrecipitation;
                    report.Description = best.Description;

                    report.CalendarExport = GenerateGoogleCalendarExport(timeOfBest, entry.Key, best);

                    // apply any warnings
                    if (best.CloudCoveragePercent > 50)
                    {
                        report.WeatherWarnings = "Cloud coverage >50%, observations may be difficult";
                    }
                    else if (best.CloudCoveragePercent > 90)
                    {
                        report.WeatherWarnings = "Cloud coverage >90%, extreme visibility degradation";
                    }

                    if (best.ProbabilityOfPrecipitation > 50)
                    {
                        report.WeatherWarnings += "\nHigh chance of rain";
                    }

                    List<SingleObjectReport> wholeList = new();

                    // for every observation this location has
                    foreach (Observation obs in entry.Value)
                    {
                        AltAz apparentHorizontalPosition = CoordinateTransformations.EquatorialToHorizontal(
                            obs.AstronomicalObject,
                            timeOfBest,
                            entry.Key.Longitude,
                            entry.Key.Latitude);

                        wholeList.Add(new SingleObjectReport()
                        {
                            Type = obs.AstronomicalObject.Type,
                            AstronomicalObjectName = obs.AstronomicalObject.Name,
                            Azimuth = apparentHorizontalPosition.Azimuth,
                            Altitude = apparentHorizontalPosition.Altitude,
                            // if object below horizon, apply a warning
                            Warning = apparentHorizontalPosition.Altitude < 0 ? "Not visible" : null
                        });

                        // create a report entity and persist it to the database
                        string type = obs.AstronomicalObject.Type == "Star" ? "Star" : "DeepSky";

                        Report persistedReport = new()
                        {
                            ObservationLocation = entry.Key,
                            AstroCueUser = user,
                            AstronomicalObjectName = obs.AstronomicalObject.Name,
                            BestTimeToObserveUtc = timeOfBest,
                            HorizontalCoordinates = apparentHorizontalPosition,
                            WeatherForecast = new WeatherForecast()
                            {
                                Description = best.Description,
                                CloudCoveragePercent = best.CloudCoveragePercent,
                                TemperatureCelcius = best.TemperatureCelcius,
                                WindSpeedMetersPerSec = best.WindSpeedMetersPerSec,
                                ProbabilityOfPrecipitation = best.ProbabilityOfPrecipitation,
                                HumidityPercent = best.HumidityPercent,
                                Sunrise = forecastForLocation.Sunrise,
                                Sunset = forecastForLocation.Sunset
                            },
                            MoreInformationUrl = ObservationService.GenerateMoreInformationUrl(type, obs.AstronomicalObject.CatalogueIdentifier)
                        };

                        this._context.Reports.Add(persistedReport);
                        this._context.SaveChanges();
                    }

                    report.Objects = wholeList
                        .Select((x, i) => new { Index = i, Value = x })
                        .GroupBy(x => x.Index / 3)
                        .Select(x => x.Select(v => v.Value).ToList())
                        .ToList();

                    // add this location's report
                    reportList.Add(report);
                }

                // here, all of the user's locations have had reports generated for all of their objects, so 
                // we can email them
                await this._emailService.SendReportEmail(user, reportList, staticMaps);
            }
        }

        /// <summary>
        /// Retrieve all of the reports that belong to a given user
        /// </summary>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <returns>A list of <see cref="OutboundReportModel"/> instances</returns>
        public IList<OutboundReportModel> GetReports(int reqUserId)
        {
            AstroCueUser user = this._context.AstroCueUsers
                .Include(u => u.Reports
                    .OrderByDescending(r => r.BestTimeToObserveUtc))
                .ThenInclude(r => r.ObservationLocation)
                .Single(u => u.Id == reqUserId);

            return this._mapper.Map<IList<OutboundReportModel>>(user.Reports);
        }

        /// <summary>
        /// Generate a Google Calendar export link for an observation
        /// </summary>
        /// <param name="bestTime">An instance of <see cref="DateTime"/></param>
        /// <param name="observationLocation">The location of the observation (<see cref="ObservationLocation"/>)</param>
        /// <param name="forecast">An <see cref="HourlyForecast"/> instance</param>
        /// <returns>A Google Calendar URL</returns>
        private static string GenerateGoogleCalendarExport(DateTime bestTime, ObservationLocation observationLocation, HourlyForecast forecast)
        {
            string text = HttpUtility.UrlEncode($"AstroCue observation at \"{observationLocation.Name}\"");

            string details = HttpUtility.UrlEncode(
                $"WEATHER: {forecast.Description}\n\n" +
                $"Cloud coverage: {forecast.CloudCoveragePercent}%\n" +
                $"Temperature: {forecast.TemperatureCelcius}°C\n" +
                $"Humidity: {forecast.HumidityPercent}%\n" +
                $"Wind speed: {forecast.WindSpeedMetersPerSec} m/s\n" +
                $"Probability of precipitation: {forecast.ProbabilityOfPrecipitation}%z\n\n");

            string location = HttpUtility.UrlEncode($"{observationLocation.Latitude},{observationLocation.Longitude}");

            string dates = HttpUtility.UrlEncode($"{bestTime:yyyyMMddTHHmmssZ}/{bestTime:yyyyMMddTHHmmssZ}");

            return "https://www.google.com/calendar/render?action=TEMPLATE" +
                   $"&text={text}" +
                   $"&details={details}" +
                   $"&location={location}" +
                   $"&dates={dates}";
        }
    }
}
