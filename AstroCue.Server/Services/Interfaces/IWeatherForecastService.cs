namespace AstroCue.Server.Services.Interfaces
{
    using System.Threading.Tasks;
    using Models.Misc;

    /// <summary>
    /// Interface for <see cref="WeatherForecastService"/>
    /// </summary>
    public interface IWeatherForecastService
    {
        /// <summary>
        /// Get a simple, on the spot weather forecast for a given location on Earth
        /// </summary>
        /// <param name="longitude">A longitude</param>
        /// <param name="latitude">A latitude</param>
        /// <returns>An instance of <see cref="SingleForecast"/></returns>
        Task<SingleForecast> GetCurrentWeatherAsync(float longitude, float latitude);

        /// <summary>
        /// Gets a set of weather forecasts for each hour over the next 4 days (for a total of
        /// 96 individual forecasts)
        /// </summary>
        /// <param name="longitude">A longitude</param>
        /// <param name="latitude">A latitude</param>
        /// <returns>An instance of <see cref="FourDayForecastReport"/></returns>
        Task<FourDayForecastReport> GetForecastNextFourDaysAsync(float longitude, float latitude);
    }
}