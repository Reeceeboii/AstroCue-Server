namespace AstroCue.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Interfaces;
    using Interfaces;
    using Models.Misc;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using Utilities;

    /// <summary>
    /// Class that handles interacting with the OpenWeatherMap API to retrieve weather forecasts
    /// </summary>
    public class WeatherForecastService : IWeatherForecastService
    {
        /// <summary>
        /// Instance of <see cref="RestClient"/>
        /// </summary>
        private readonly RestClient _client;

        /// <summary>
        /// Instance of <see cref="IEnvironmentManager"/>
        /// </summary>
        private readonly IEnvironmentManager _environmentManager;

        /// <summary>
        /// Initialises a new instance of the <see cref="WeatherForecastService"/> class
        /// </summary>
        /// <param name="environmentManager">Instance of <see cref="IEnvironmentManager"/></param>
        /// <param name="httpClientService">Instance of <see cref="IHttpClientService"/></param>
        public WeatherForecastService(IEnvironmentManager environmentManager, IHttpClientService httpClientService)
        {
            this._client = httpClientService.NewClient();
            this._environmentManager = environmentManager;
        }

        /// <summary>
        /// Get a simple, on the spot weather forecast for a given location on Earth
        /// </summary>
        /// <param name="longitude">A longitude</param>
        /// <param name="latitude">A latitude</param>
        /// <returns>An instance of <see cref="SingleForecast"/></returns>
        public async Task<SingleForecast> GetCurrentWeatherAsync(float longitude, float latitude)
        {
            RestRequest req = new(this._environmentManager.BaseOwmCurrentWeatherUrl);

            req.AddParameter("lat", latitude);
            req.AddParameter("lon", longitude);
            req.AddParameter("units", "metric");
            req.AddParameter("appid", this._environmentManager.OpenWeatherMapApiKey);

            RestResponse res = await this._client.GetAsync(req);

            if (res.Content == null)
            {
                return new SingleForecast
                {
                    Description = "Data could not be retrieved"
                };
            }

            JObject jObject = JObject.Parse(res.Content);

            return new SingleForecast
            {
                Description = StringUtilities.TrimToUpperFirstChar((string)jObject["weather"]![0]!["description"]),
                CloudCoveragePercent = (int)jObject["clouds"]!["all"],
                TemperatureCelcius = Convert.ToInt32((int)jObject["main"]!["temp"]),
                RetrievedAt = DateTime.Now.ToString("s")
            };
        }

        /// <summary>
        /// Gets a set of weather forecasts for each hour over the next 4 days (for a total of
        /// 96 individual forecasts)
        /// </summary>
        /// <param name="longitude">A longitude</param>
        /// <param name="latitude">A latitude</param>
        /// <returns>An instance of <see cref="FourDayForecastReport"/></returns>
        public async Task<FourDayForecastReport> GetForecastNextFourDaysAsync(float longitude,
            float latitude)
        {
            RestRequest req = new(this._environmentManager.BaseOwnHourlyForecastUrl);

            req.AddParameter("lat", latitude);
            req.AddParameter("lon", longitude);
            req.AddParameter("units", "metric");
            req.AddParameter("appid", this._environmentManager.OpenWeatherMapApiKey);

            RestResponse res = await this._client.GetAsync(req);

            if (res.Content == null)
            {
                throw new Exception("Data could not be retrieved");
            }

            Dictionary<EqTimeRecord, HourlyForecast> forecasts = new();
            FourDayForecastReport forecast = new();

            JObject jObject = JObject.Parse(res.Content);

            // extract the hourly forecast list
            IList<JToken> hours = jObject["list"]!.Children().ToList();

            foreach (JToken hour in hours)
            {
                DateTime time = hour["dt_txt"]!.ToObject<DateTime>();
                int cloudCoveragePercent = (int)hour["clouds"]!["all"];
                int temperatureCelcius = Convert.ToInt32((int)hour["main"]!["temp"]);
                int humidityPercent = (int)hour["main"]["humidity"];
                float windSpeedMetersPerSec = (float)hour["wind"]!["speed"];
                float probabilityOfPrecipitation = (float)hour["pop"];
                string description = StringUtilities.TrimToUpperFirstChar((string)hour["weather"]![0]!["description"]);

                EqTimeRecord eq = new()
                {
                    Day = time.Day,
                    Hour = time.Hour
                };

                forecasts.Add(eq, new HourlyForecast()
                {
                    CloudCoveragePercent = cloudCoveragePercent,
                    TemperatureCelcius = temperatureCelcius,
                    HumidityPercent = humidityPercent,
                    WindSpeedMetersPerSec = windSpeedMetersPerSec,
                    ProbabilityOfPrecipitation = probabilityOfPrecipitation,
                    Description = description
                });
            }

            forecast.Forecasts = forecasts;

            long sunrise = (long)jObject["city"]!["sunrise"];
            long sunset = (long)jObject["city"]!["sunset"];
            DateTimeOffset sunriseOffset = DateTimeOffset.FromUnixTimeSeconds(sunrise);
            DateTimeOffset sunsetOffset = DateTimeOffset.FromUnixTimeSeconds(sunset);

            forecast.Sunrise = sunriseOffset.DateTime;
            forecast.Sunset = sunsetOffset.DateTime;

            return forecast;
        }
    }
}
