namespace AstroCue.Server.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Owned;

    /// <summary>
    /// Entity class representing an astronomical observation log taken by a user.
    /// Many of the fields in this entity are copies of data rather than FK links to existing instances
    /// in other tables. This is so that even if reports and observation locations and reports are deleted,
    /// any logs created from their existence can be retained
    /// </summary>
    public class ObservationLog
    {
        /// <summary>
        /// Gets or sets the ID of the observation log
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a textual description of what was observed
        /// </summary>
        [Required]
        [MaxLength(2000)]
        public string TextualDescription { get; set; }

        /// <summary>
        /// Gets or sets the name(s) of those who carried out the observation
        /// </summary>
        [MaxLength(250)]
        public string Observer { get; set; }

        /// <summary>
        /// Gets or sets the weather forecast at the time of the observation
        /// </summary>
        [Required]
        public WeatherForecast WeatherForecast { get; set; }

        /// <summary>
        /// Gets or sets the name of the observation location where the observation was
        /// carried out
        /// </summary>
        [Required]
        public string ObservationLocationName { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the observation location where the observation was
        /// carried out
        /// </summary>
        [Required]
        public float ObservationLocationLatitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the observation location where the observation was
        /// carried out
        /// </summary>
        [Required]
        public float ObservationLocationLongitude { get; set; }

        /// <summary>
        /// Gets or sets the name of the astronomical object that was observed
        /// </summary>
        [Required]
        public string ObservedAstronomicalObjectName { get; set; }

        /// <summary>
        /// Gets or sets the time that was calculated as the optimal observation time
        /// </summary>
        [Required]
        public DateTime CalculatedBestTimeToObserveUtc { get; set; }

        /// <summary>
        /// Gets or sets the horizontal coordinates at which the object was calculated to be at
        /// at the time of the observation
        /// </summary>
        [Required]
        public AltAz HorizontalCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the type of observation that was carried out
        /// </summary>
        [Required]
        public string TypeOfObservation { get; set; }

        /// <summary>
        /// Gets or sets the date that the observation log was edited.
        /// This is set by the server itself and is used to order logs by time when they are
        /// retrieved
        /// </summary>
        [Required]
        public DateTime DateLastEditedUtc { get; set; }

        /// <summary>
        /// Gets or sets the ID of the report that this log was taken for.
        /// This is not set up as a FK relationship so that the report itself
        /// can be deleted while the log is retained. This is simply for preventing
        /// duplicate logs being taken against reports that do still exist.
        /// </summary>
        [Required]
        public int LogForReportId { get; set; }
    }
}
