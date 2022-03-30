namespace AstroCue.Server.Models.Misc
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Model class representing a 4 day long forecast from the OpenWeatherMap API
    /// </summary>
    public class FourDayForecastReport
    {
        /// <summary>
        /// Gets or sets a dictionary mapping hourly timestamps to <see cref="HourlyForecast"/>
        /// instances
        /// </summary>
        public IDictionary<EqTimeRecord, HourlyForecast> Forecasts { get; set; }

        /// <summary>
        /// Gets or sets the sunset timestamp
        /// </summary>
        public DateTime Sunset { get; set; }

        /// <summary>
        /// Gets or sets the sunrise timestamp
        /// </summary>
        public DateTime Sunrise { get; set; }
    }
}
