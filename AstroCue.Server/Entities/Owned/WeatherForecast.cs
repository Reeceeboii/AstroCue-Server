namespace AstroCue.Server.Entities.Owned
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Entity class representing a weather forecast for a particular time and location.
    /// This entity however, just stores the weather details themselves. The time and location details will be stored
    /// by the parent entity.
    /// </summary>
    [Owned]
    public class WeatherForecast
    {
        /// <summary>
        /// Gets or sets the cloud coverage (0-100%)
        /// </summary>
        [Required]
        public int CloudCoveragePercent { get; set; }

        /// <summary>
        /// Gets or sets the temperature, in degrees C
        /// </summary>
        [Required]
        public int TemperatureCelcius { get; set; }

        /// <summary>
        /// Gets or sets the humidty (0-100%)
        /// </summary>
        [Required]
        public int HumidityPercent { get; set; }

        /// <summary>
        /// Gets or sets the wind speed in m/s
        /// </summary>
        [Required]
        public float WindSpeedMetersPerSec { get; set; }

        /// <summary>
        /// Gets or sets the probability of precipitation (0-1)
        /// </summary>
        [Required]
        public float ProbabilityOfPrecipitation { get; set; }

        /// <summary>
        /// Gets or sets a textual description of the weather conditions
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the sunset timestamp
        /// </summary>
        [Required]
        public DateTime Sunset { get; set; }

        /// <summary>
        /// Gets or sets the sunrise timestamp
        /// </summary>
        [Required]
        public DateTime Sunrise { get; set; }
    }
}
