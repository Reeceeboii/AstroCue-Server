namespace AstroCue.Server.Entities.Owned
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Entity class to represent a pair of coordinates in the horizontal coordinate system
    /// https://en.wikipedia.org/wiki/Horizontal_coordinate_system
    /// </summary>
    [Owned]
    public class AltAz
    {
        /// <summary>
        /// Gets or sets the altitude (-90-90 deg)
        /// </summary>
        [Required]
        public float Altitude { get; set; }

        /// <summary>
        /// Gets or sets the azimuth (0-360 deg increasing Westwards)
        /// </summary>
        [Required]
        public float Azimuth { get; set; }
    }
}
