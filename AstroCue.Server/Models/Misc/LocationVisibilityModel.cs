namespace AstroCue.Server.Models.Misc
{
    /// <summary>
    /// Model to represent a small visibility report about an astronomical object.
    /// Uses visible magnitude from the UBV photometric system and a series of
    /// diurnal motion calculations (see <see cref="Services.ObservationService"/>)
    /// </summary>
    public class LocationVisibilityModel
    {
        /// <summary>
        /// Gets or sets a boolean representing whether or not there is an alert
        /// on the visibility of an object
        /// </summary>
        public bool VisibilityAlert { get; set; }

        /// <summary>
        /// Gets or sets a boolean representing whether or not there is an alert
        /// regarding the object's (lack of) rise above the observer's
        /// local horizon (calculated via diurnal motion)
        /// </summary>
        public bool HorizonAlert { get; set; }

        /// <summary>
        /// Gets or sets a message regarding the visibility of the object
        /// </summary>
        public string VisibilityMessage { get; set; }

        /// <summary>
        /// Gets or sets a message regarding the diurnal motion of the object
        /// </summary>
        public string HorizonMessage { get; set; }
    }
}
