namespace AstroCue.Server.Data.Interfaces
{
    /// <summary>
    /// Interface for <see cref="EnvironmentManager"/>
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
    }
}