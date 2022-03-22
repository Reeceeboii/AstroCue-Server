namespace AstroCue.Server.Models.API.Inbound
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Model class representing a new incoming observation location
    /// </summary>
    public class InboundObsLocationModel
    {
        /// <summary>
        /// Gets or sets the user's chosen name of the new observation location
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Name must be no more than 50 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the new observation location
        /// </summary>
        [Required]
        [Range(-180f, 180f, ErrorMessage = "Longitude must be valid (-180° to +180°)")]
        public float Longitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the new observation location
        /// </summary>
        [Required]
        [Range(-90f, 90f, ErrorMessage = "Latitude must be valid (-90° to +90°)")]
        public float Latitude { get; set; }
    }
}
