namespace AstroCue.Server.Models.API.Outbound
{
    using Misc;

    /// <summary>
    /// Model class representing an astronomical object
    /// </summary>
    public class OutboundAstronomicalObjectModel
    {
        /// <summary>
        /// Gets or sets the ID for the astronomical object
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code for an astronomical object from its catalogue entry.
        /// I.e. entry HIP 12345 will have code 12345
        /// </summary>
        public int CatalogueIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the name for a star (i.e. Polaris, Betelgeuse etc...). As only a tiny fraction of stars
        /// have officially recognised names, this prop is nullable and will be so for an overwhelmingly large
        /// percentage of entries
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the object  
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the apparent magnitude of the object
        /// </summary>
        public float ApparentMagnitude { get; set; }

        /// <summary>
        /// Gets or sets an instance of <see cref="Misc.LocationVisibilityModel"/>
        /// </summary>
        public LocationVisibilityModel LocationVisibilityModel { get; set; }

        /// <summary>
        /// Gets or sets a link where users can discover more information about the object
        /// </summary>
        public string MoreInformation { get; set; }
    }
}
