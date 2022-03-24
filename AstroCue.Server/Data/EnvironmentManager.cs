namespace AstroCue.Server.Data
{
    using Interfaces;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Class for managing and injecting testable environment variables.
    /// Injection can be done by mocking <see cref="IEnvironmentManager"/>
    /// </summary>
    public class EnvironmentManager : IEnvironmentManager
    {
        /// <summary>
        /// Instance of <see cref="IConfiguration"/>
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initialises a new instance of <see cref="EnvironmentManager"/>
        /// </summary>
        /// <param name="configuration">Instance of <see cref="IConfiguration"/></param>
        public EnvironmentManager(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        #region Environment Variables

        /// <summary>
        /// Gets the server's JSON web token encryption secret from the environment
        /// </summary>
        public string JwtSecret => this._configuration["AppSettings:JsonWebTokenSecret"];

        /// <summary>
        /// Gets the MapBox API key from the environment
        /// </summary>
        public string MapBoxApiKey => this._configuration["MapBox:APIKey"];

        /// <summary>
        /// Gets the OpenWeatherMap API key from the environment
        /// </summary>
        public string OpenWeatherMapApiKey => this._configuration["OpenWeatherMap:APIKey"];

        /// <summary>
        /// Gets the MailGun API key from the environment
        /// </summary>
        public string MailGunApiKey => this._configuration["MailGun:APIKey"];

        #endregion // Environment Variables

        #region Base API URLs

        /// <summary>
        /// Gets the base messages URL for the MailGun API
        /// </summary>
        public string BaseMailGunMessagesUrl => "https://api.eu.mailgun.net/v3";

        /// <summary>
        /// Base URL of the MapBox forward geocoding API endpoint
        /// </summary>
        public string BaseMapBoxForwardGeocodeUrl => "https://api.mapbox.com/geocoding/v5/mapbox.places/";

        /// <summary>
        /// Base URL of the MapBox static map images API endpoint
        /// </summary>
        public string BaseMapBoxStaticMapsUrl => "https://api.mapbox.com/styles/v1/mapbox/dark-v10/static/";

        /// <summary>
        /// Base URL of the OpenWeatherMap current weather URL endpoint
        /// </summary>
        public string BaseOwmCurrentWeatherUrl => "https://api.openweathermap.org/data/2.5/weather/";

        /// <summary>
        /// Gets the base URL of the OpenWeatherMap hourly forecast API endpoint
        /// </summary>
        public string BaseOwnHourlyForecastUrl => "https://pro.openweathermap.org/data/2.5/forecast/hourly";

        #endregion // Base API URLs
    }
}
