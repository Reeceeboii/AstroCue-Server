namespace AstroCue.Server.Models.API.Outbound
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an outbound observation location with an attached list
    /// of <see cref="OutboundReportModel"/> instances
    /// </summary>
    public class OutboundObsLocReportModel
    {
        /// <summary>
        /// Gets or sets the ID of the observation location
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the observation location - set by a user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the observation location
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the observation location
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// Gets or sets the Bortle Scale value for this location
        /// https://en.wikipedia.org/wiki/Bortle_scale
        /// </summary>
        public int BortleScaleValue { get; set; }

        /// <summary>
        /// Gets or sets a textual description of the location's Bortle Scale value
        /// </summary>
        public string BortleDesc { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="OutboundReportModel"/> instances
        /// attached to this location
        /// </summary>
        public IList<OutboundReportModel> Reports { get; set; }
    }
}
