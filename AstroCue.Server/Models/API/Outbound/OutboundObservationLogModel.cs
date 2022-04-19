namespace AstroCue.Server.Models.API.Outbound
{
    using System;
    using Entities.Owned;

    /// <summary>
    /// Model class representing an outbound observation log returned to the client
    /// </summary>
    public class OutboundObservationLogModel
    {
        /// <summary>
        /// Gets or sets the ID of the observation log
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a textual description of what was observed
        /// </summary>
        public string TextualDescription { get; set; }

        /// <summary>
        /// Gets or sets the name(s) of those who carried out the observation
        /// </summary>
        public string Observer { get; set; }

        /// <summary>
        /// Gets or sets the weather forecast at the time of the observation
        /// </summary>
        public WeatherForecast WeatherForecast { get; set; }

        /// <summary>
        /// Gets or sets the name of the observation location where the observation was
        /// carried out
        /// </summary>
        public string ObservationLocationName { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the observation location where the observation was
        /// carried out
        /// </summary>
        public float ObservationLocationLatitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the observation location where the observation was
        /// carried out
        /// </summary>
        public float ObservationLocationLongitude { get; set; }

        /// <summary>
        /// Gets or sets the name of the astronomical object that was observed
        /// </summary>
        public string ObservedAstronomicalObjectName { get; set; }

        /// <summary>
        /// Gets or sets the time that was calculated as the optimal observation time
        /// </summary>
        public DateTime CalculatedBestTimeToObserveUtc { get; set; }

        /// <summary>
        /// Gets or sets the horizontal coordinates at which the object was calculated to be at
        /// at the time of the observation
        /// </summary>
        public AltAz HorizontalCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the type of observation that was carried out
        /// </summary>
        public string TypeOfObservation { get; set; }
    }
}
