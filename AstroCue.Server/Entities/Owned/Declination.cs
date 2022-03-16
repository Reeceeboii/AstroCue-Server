namespace AstroCue.Server.Entities.Owned
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Class to represent right declination values, i.e. DEC +07° 24′ 25.4304″
    /// https://en.wikipedia.org/wiki/Declination
    /// </summary>
    [Owned]
    public class Declination
    {
        /// <summary>
        /// Gets or sets the declination's degrees value
        /// </summary>
        [Required]
        public int Degrees { get; set; }

        /// <summary>
        /// Gets or sets the declination's minutes value
        /// </summary>
        [Required]
        public int Minutes { get; set; }

        /// <summary>
        /// Gets or sets the declination's seconds value
        /// </summary>
        [Required]
        public double Seconds { get; set; }
    }
}
