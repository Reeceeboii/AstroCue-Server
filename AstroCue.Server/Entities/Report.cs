namespace AstroCue.Server.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Owned;

    /// <summary>
    /// Entity class representing a report on the best time to go and observe an object
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Gets or sets the Id  of this observation report
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Entities.ObservationLocation"/> navigation property
        /// </summary>
        public ObservationLocation ObservationLocation { get; set; }

        /// <summary>
        /// Gets or sets the name of the astronomical object that was observed
        /// </summary>
        [Required]
        public string AstronomicalObjectName { get; set; }

        /// <summary>
        /// Gets or sets the ID of the astronomical object
        /// that the report is targeting
        /// </summary>
        [Required]
        public int AstronomicalObjectReportIsTargeting { get; set; }

        /// <summary>
        /// Gets or sets the best time to observe that was calculated (in UTC)
        /// </summary>
        [Required]
        public DateTime BestTimeToObserveUtc { get; set; }

        /// <summary>
        /// Gets or sets a link where users can discover more information about the object
        /// </summary>
        [Required]
        public string MoreInformationUrl { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="WeatherForecast"/> at the time of the observation
        /// </summary>
        [Required]
        public WeatherForecast WeatherForecast { get; set; }

        /// <summary>
        /// Gets or sets the horizontal coordinates at which the object was calculated to be at
        /// at the instant of <see cref="BestTimeToObserveUtc"/>
        /// </summary>
        [Required]
        public AltAz HorizontalCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Entities.AstroCueUser"/> foreign key
        /// </summary>
        public int AstroCueUserId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Entities.AstroCueUser"/> navigation property
        /// </summary>
        public AstroCueUser AstroCueUser { get; set; }
    }
}
