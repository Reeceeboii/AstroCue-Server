namespace AstroCue.Server.Entities
{
    /// <summary>
    /// Entity class representing a single astronomical observation.
    /// (Conceptually created when a user adds an <see cref="Entities.AstronomicalObject"/>
    /// to an <see cref="Entities.ObservationLocation"/>).
    ///
    /// Used as a joining/linking DB table for a many-to-many
    /// relationship between these two entities
    /// </summary>
    public class Observation
    {
        /// <summary>
        /// Gets or sets the Id of the astronomical observation
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Entities.ObservationLocation"/> foreign key
        /// </summary>
        public int ObservationLocationId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Entities.ObservationLocation"/> navigation property
        /// </summary>
        public ObservationLocation ObservationLocation { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Entities.AstronomicalObject"/> foreign key
        /// </summary>
        public int AstronomicalObjectId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Entities.AstronomicalObject"/> navigation property
        /// </summary>
        public AstronomicalObject AstronomicalObject { get; set; }
    }
}
