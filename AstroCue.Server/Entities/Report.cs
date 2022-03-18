namespace AstroCue.Server.Entities
{
    /// <summary>
    /// Entity class representing a report on the best time to go and observe an object
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Gets or sets the Id  of this observation report
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Entities.ObservationLocation"/> navigation property
        /// </summary>
        public ObservationLocation ObservationLocation { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Entities.AstroCueUser"/> foreign key
        /// </summary>
        public int AstroCueUserId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Entities.AstroCueUser"/> navigation property
        /// </summary>
        public AstroCueUser AstroCueUser { get; set; }
    }
}
