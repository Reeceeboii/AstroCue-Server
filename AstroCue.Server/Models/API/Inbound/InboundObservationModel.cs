namespace AstroCue.Server.Models.API.Inbound
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Model class representing a new incoming astronomical observation
    /// </summary>
    public class InboundObservationModel
    {
        /// <summary>
        /// Gets or sets the ID of the <see cref="Entities.ObservationLocation"/> that
        /// the observation will take place at
        /// </summary>
        [Required]
        public int LocationId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the <see cref="Entities.AstronomicalObject"/> that the
        /// observation will target
        /// </summary>
        [Required]
        public int AstronomicalObjectId { get; set; }
    }
}
