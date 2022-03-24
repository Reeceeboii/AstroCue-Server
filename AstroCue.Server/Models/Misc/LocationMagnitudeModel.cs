namespace AstroCue.Server.Models.Misc
{
    /// <summary>
    /// Model to represent a small visibility report about an astronomical object.
    /// Uses visible magnitude from the UBV photometric system
    /// </summary>
    public class LocationMagnitudeModel
    {
        /// <summary>
        /// Gets or sets a boolean representing whether or not there is an alert
        /// on the visibility of an object
        /// </summary>
        public bool VisibilityAlert { get; set; }

        /// <summary>
        /// Gets or sets a message regarding the visibility of the object
        /// </summary>
        public string VisibilityMessage { get; set; }
    }
}
