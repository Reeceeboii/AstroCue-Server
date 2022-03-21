namespace AstroCue.Server.Data.Interfaces
{
    /// <summary>
    /// Interface for <see cref="EnvironmentManager"/>.
    /// Allows dependency injection and mocking of environment variables and API URLs
    /// </summary>
    public interface IEnvironmentManager
    {
        /// <summary>
        /// Gets the server's JSON web token encryption secret from the environment
        /// </summary>
        string JwtSecret { get; }

        /// <summary>
        /// Gets the MapBox API key from the environment
        /// </summary>
        string MapBoxApiKey { get; }

        /// <summary>
        /// Gets the OpenWeatherMap API key from the environment
        /// </summary>
        string OpenWeatherMapApiKey { get; }

        /// <summary>
        /// Gets the MailGun API key from the environment
        /// </summary>
        string MailGunApiKey { get; }

        /// <summary>
        /// Gets the base messages URL for the MailGun API
        /// </summary>
        string BaseMailGunMessagesUrl { get; }

        /// <summary>
        /// Base URL of the MapBox forward geocoding API endpoint
        /// </summary>
        string BaseMapBoxForwardGeocodeUrl { get; }

        /// <summary>
        /// Base URL of the MapBox static map images API endpoint
        /// </summary>
        string BaseMapBoxStaticMapsUrl { get; }
    }
}