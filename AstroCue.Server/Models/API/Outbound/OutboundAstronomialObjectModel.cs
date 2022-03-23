namespace AstroCue.Server.Models.API.Outbound
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Model class representing an astronomical object
    /// </summary>
    public class OutboundAstronomialObjectModel
    {
        /// <summary>
        /// Gets or sets the ID for the astronomical object
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code for an astronomical object from its catalogue entry.
        /// I.e. entry HIP 12345 will have code 12345
        /// </summary>
        [Required]
        public int CatalogueIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the name for a star (i.e. Polaris, Betelgeuse etc...). As only a tiny fraction of stars
        /// have officially recognised names, this prop is nullable and will be so for an overwhelmingly large
        /// percentage of entries
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the object  
        /// </summary>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the apparent magnitude of the object
        /// </summary>
        [Required]
        public float ApparentMagnitude { get; set; }

        /// <summary>
        /// Gets or sets a boolean representing whether or not there is an alert
        /// on the visibility of an object based on the location provided via
        /// the locationId provided in the request
        /// </summary>
        public bool VisibilityAlert { get; set; }
        
        /// <summary>
        /// Gets or sets a message regarding the visibility of the object
        /// </summary>
        public string VisibilityMessage { get; set; }

        /// <summary>
        /// Gets or sets a link where users can discover more information about the object
        /// </summary>
        public string MoreInformation { get; set; }
    }
}
