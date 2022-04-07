namespace AstroCue.Server.Models.API.Outbound
{
    /// <summary>
    /// Model class representing an astronomical observation
    /// </summary>
    public class OutboundObservationModel
    {
        /// <summary>
        /// Gets or sets the Id of the astronomical observation
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the <see cref="Entities.ObservationLocation"/> that this
        /// observation takes place at
        /// </summary>
        public int ObservationLocationId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Entities.ObservationLocation"/> that this observation takes
        /// place at
        /// </summary>
        public OutboundObsLocationModel ObservationLocation { get; set; }

        /// <summary>
        /// Gets or sets the ID of the <see cref="Entities.AstronomicalObject"/> that this
        /// observatio is targeting
        /// </summary>
        public int AstronomicalObjectId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Entities.AstronomicalObject"/> that this 
        /// observation is targeting
        /// </summary>
        public OutboundAstronomicalObjectModel AstronomicalObject { get; set; }
    }
}
