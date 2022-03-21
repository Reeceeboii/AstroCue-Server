namespace AstroCue.Server.Services
{
    using System;
    using System.Threading.Tasks;
    using Data.Interfaces;
    using Interfaces;
    using Models.Misc;
    using Newtonsoft.Json.Linq;
    using RestSharp;

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
                Description = (string)jObject["weather"][0]["description"],
                CloudCoveragePercent = (int)jObject["clouds"]["all"],
                TemperatureCelcius = Convert.ToInt32((int)jObject["main"]["temp"]),
                RetrievedAt = DateTime.Now.ToString("s")
            };
        }
    }
}
