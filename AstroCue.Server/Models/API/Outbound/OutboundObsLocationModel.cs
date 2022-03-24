namespace AstroCue.Server.Models.API.Outbound
{
    using System.ComponentModel.DataAnnotations;
    using Misc;

    /// <summary>
    /// Model class representing an observation location
    /// </summary>
    public class OutboundObsLocationModel
    {
        /// <summary>
        /// Gets or sets the ID of the observation location
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the observation location - set by a user
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the observation location
        /// </summary>
        [Range(-180f, 180f)]
        public float Longitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the observation location
        /// </summary>
        [Range(-90f, 90f)]
        public float Latitude { get; set; }

        /// <summary>
        /// Gets or sets the Bortle Scale value for this location
        /// https://en.wikipedia.org/wiki/Bortle_scale
        /// </summary>
        [Range(1, 8)]
        public int BortleScaleValue { get; set; }

        /// <summary>
        /// Gets or sets a textual description of the location's Bortle Scale value
        /// </summary>
        public string BortleDesc { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="SingleForecast"/> for this location
        /// </summary>
        public SingleForecast SingleForecast { get; set; }
    }
}
