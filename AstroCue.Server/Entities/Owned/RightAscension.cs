namespace AstroCue.Server.Entities.Owned
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Class to represent right ascension values, i.e. RA 05h 55m 10.30536s
    /// https://en.wikipedia.org/wiki/Right_ascension
    /// </summary>
    [Owned]
    public class RightAscension
    {
        /// <summary>
        /// Gets or sets the right ascension's hours value
        /// </summary>
        [Required]
        public int Hours { get; set; }

        /// <summary>
        /// Gets or sets the right ascension's minutes value
        /// </summary>
        [Required]
        public int Minutes { get; set; }

        /// <summary>
        /// Gets or sets the right ascension's seconds value
        /// </summary>
        [Required]
        public double Seconds { get; set; }
    }
}
