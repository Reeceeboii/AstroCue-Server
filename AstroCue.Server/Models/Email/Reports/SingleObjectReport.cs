namespace AstroCue.Server.Models.Email.Reports
{
    /// <summary>
    /// Model class representing a single object on a single day
    /// as part of a report email
    /// </summary>
    public class SingleObjectReport
    {
        /// <summary>
        /// Gets or sets the name of the object
        /// </summary>
        public string AstronomicalObjectName { get; set; }

        /// <summary>
        /// Gets or sets the type of the object
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the object's altitude at the recommended time of observation
        /// </summary>
        public float Altitude { get; set; }

        /// <summary>
        /// Gets or sets the object's azimuth at the recommended time of observation
        /// </summary>
        public float Azimuth { get; set; }

        /// <summary>
        /// Gets or sets a warning about the object. This can be null
        /// </summary>
        public string Warning { get; set; }
    }
}
