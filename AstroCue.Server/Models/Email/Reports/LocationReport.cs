namespace AstroCue.Server.Models.Email.Reports
{
    using System.Collections.Generic;

    /// <summary>
    /// Model class representing a single location send
    /// in a report email
    /// </summary>
    public class LocationReport
    {
        /// <summary>
        /// Gets or sets the name of the location
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the inline map image for this location
        /// </summary>
        public string StaticMapImageName { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the observation location
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the observation location
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// Gets or sets the best time to observe on this particular day 
        /// </summary>
        public string BestTimeToObserve { get; set; }

        /// <summary>
        /// Gets or sets the cloud coverage (0-100%)
        /// </summary>
        public int CloudCoveragePercent { get; set; }

        /// <summary>
        /// Gets or sets the temperature, in degrees C
        /// </summary>
        public int TemperatureCelcius { get; set; }

        /// <summary>
        /// Gets or sets the humidty (0-100%)
        /// </summary>
        public int HumidityPercent { get; set; }

        /// <summary>
        /// Gets or sets the wind speed in m/s
        /// </summary>
        public float WindSpeedMetersPerSec { get; set; }

        /// <summary>
        /// Gets or sets the probability of precipitation (0-1)
        /// </summary>
        public float ProbabilityOfPrecipitation { get; set; }

        /// <summary>
        /// Gets or sets a textual description of the weather conditions
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets weather warnings for this day
        /// </summary>
        public string WeatherWarnings { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="SingleObjectReport"/> instances
        /// </summary>
        public IList<SingleObjectReport> Objects { get; set; }

        /// <summary>
        /// Gets or sets the location's sunrise time
        /// </summary>
        public string Sunrise { get; set; }

        /// <summary>
        /// Gets or sets the location's sunset time
        /// </summary>
        public string Sunset { get; set; }
    }
}
