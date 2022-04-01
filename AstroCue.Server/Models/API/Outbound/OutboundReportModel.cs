namespace AstroCue.Server.Models.API.Outbound
{
    using System;
    using Entities.Owned;

    /// <summary>
    /// Model class representing a report sent to a user
    /// </summary>
    public class OutboundReportModel
    {
        /// <summary>
        /// Gets or sets the Id  of this observation report
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Entities.ObservationLocation"/> navigation property
        /// </summary>
        public OutboundObsLocationModel ObservationLocation { get; set; }

        /// <summary>
        /// Gets or sets the name of the astronomical object that was observed
        /// </summary>
        public string AstronomicalObjectName { get; set; }

        /// <summary>
        /// Gets or sets the best time to observe that was calculated (in UTC)
        /// </summary>
        public DateTime BestTimeToObserveUtc { get; set; }

        /// <summary>
        /// Gets or sets a link where users can discover more information about the object
        /// </summary>
        public string MoreInformationUrl { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="WeatherForecast"/> at the time of the observation
        /// </summary>
        public WeatherForecast WeatherForecast { get; set; }

        /// <summary>
        /// Gets or sets the horizontal coordinates at which the object was calculated to be at
        /// at the instant of <see cref="BestTimeToObserveUtc"/>
        /// </summary>
        public AltAz HorizontalCoordinates { get; set; }
    }
}
