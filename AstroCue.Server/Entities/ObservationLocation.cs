namespace AstroCue.Server.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Entity class to represent an observation location added by an <see cref="Entities.AstroCueUser"/>
    /// </summary>
    public class ObservationLocation
    {
        /// <summary>
        /// Gets or sets the ID of the observation location
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the observation location - set by a user
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Name must be no more than 50 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the observation location
        /// </summary>
        [Required]
        [Range(-180f, 180f, ErrorMessage = "Longitude must be valid (-180° to +180°)")]
        public float Longitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the observation location
        /// </summary>
        [Required]
        [Range(-90f, 90f, ErrorMessage = "Latitude must be valid (-90° to +90°)")]
        public float Latitude { get; set; }

        /// <summary>
        /// Gets or sets the Bortle Scale value for this location
        /// https://en.wikipedia.org/wiki/Bortle_scale
        /// </summary>
        [Required]
        [Range(1, 8, ErrorMessage = "Bortle out of range")]
        public int BortleScaleValue { get; set; }

        /// <summary>
        /// Gets or sets a textual description of the location's Bortle Scale value
        /// </summary>
        [Required]
        public string BortleDesc { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Observation"/>s that this observation location
        /// forms a part of
        /// </summary>
        public List<Observation> Observations { get; set; }

        /// <summary>
        /// Gets or sets the foreign key linked to a <see cref="Entities.AstroCueUser"/>
        /// </summary>
        public int AstroCueUserId { get; set; }

        /// <summary>
        /// Gets or sets the reference navigation property
        /// </summary>
        public AstroCueUser AstroCueUser { get; set; }
    }
}
