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
    }
}
