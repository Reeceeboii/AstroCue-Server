 namespace AstroCue.Server.Controllers.GeographicController
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Query parameter binding for <see cref="GeoController.ForwardGeocode"/>
    /// </summary>
    public class FwdGeocodeParams
    {
        /// <summary>
        /// Gets or sets the search query
        /// </summary>
        [Required]
        [StringLength(256)]
        public string Query { get; set; }
    }
}
