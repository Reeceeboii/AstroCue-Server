namespace AstroCue.Server.Entities
{
    /// <summary>
    /// Entity class used to represent a deep sky object read from the New General Catalogue.
    /// Type of <see cref="AstronomicalObject"/>
    /// </summary>
    public class NgcObject : AstronomicalObject
    {
        /// <summary>
        /// Gets or sets a boolean representing whether or not this object is part of a multiple system
        /// </summary>
        public bool PartOfMultipleSystem { get; set; }
    }
}
