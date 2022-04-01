namespace AstroCue.Server.Models.Misc
{
    /// <summary>
    /// Model class to represent a weather forecast for a single hour
    /// </summary>
    public class HourlyForecast
    {
        /// <summary>
        /// Gets or sets the cloud coverage (0-100%)
        /// </summary>
        public int CloudCoveragePercent { get; init; }

        /// <summary>
        /// Gets or sets the temperature, in degrees C
        /// </summary>
        public int TemperatureCelcius { get; init; }

        /// <summary>
        /// Gets or sets the humidty (0-100%)
        /// </summary>
        public int HumidityPercent { get; init; }

        /// <summary>
        /// Gets or sets the wind speed in m/s
        /// </summary>
        public float WindSpeedMetersPerSec { get; init; }

        /// <summary>
        /// Gets or sets the probability of precipitation (0-1)
        /// </summary>
        public float ProbabilityOfPrecipitation { get; init; }

        /// <summary>
        /// Gets or sets a textual description of the weather conditions
        /// </summary>
        public string Description { get; init; }
    }
}
