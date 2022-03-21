namespace AstroCue.Server.Models.Misc
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Model representing a *simple* weather forecast for a particular time and location
    /// </summary>
    public class SingleForecast
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
        /// Gets or sets a textual description of the weather conditions
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the time that the forecast was retrieved at (ISO 8601)
        /// </summary>
        [Required]
        public string RetrievedAt { get; set; }
    }
}
