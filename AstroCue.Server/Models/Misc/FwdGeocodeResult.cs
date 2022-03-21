namespace AstroCue.Server.Models.Misc
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Model class representing a response from the MapBox forward geocode API.
    /// https://docs.mapbox.com/api/search/geocoding/
    /// </summary>
    public class FwdGeocodeResult
    {
        /// <summary>
        /// Gets or sets a string representing the feature
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a string representing the feature with its full result hierarchy
        /// </summary>
        [Required]
        public string PlaceName { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the location
        /// </summary>
        [Required]
        public float Longitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the location
        /// </summary>
        [Required]
        public float Latitude { get; set; }
    }
}
