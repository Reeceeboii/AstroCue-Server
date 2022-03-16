namespace AstroCue.Server.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Owned;

    /// <summary>
    /// Entity class representing an astronomical object read from a catalogue
    /// </summary>
    public class AstronomicalObject
    {
        /// <summary>
        /// Gets or sets the ID for the astronomical object
        /// </summary>
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
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the right ascension (Ra) for the object, measured in Hours|Minutes|Seconds from epoch J2000.0
        /// Uses complex type
        /// </summary>
        [Required]
        public RightAscension RightAscension { get; set; }

        /// <summary>
        /// Gets or sets the declination (Dec) for the object, measured in Degrees|Minutes|Seconds from epoch J2000.0
        /// Uses complex type
        /// </summary>
        [Required]
        public Declination Declination { get; set; }

        /// <summary>
        /// Gets or sets the apparent magnitude of the object
        /// </summary>
        [Required]
        public float ApparentMagnitude { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Observation"/>s that this astronomical object
        /// forms a part of
        /// </summary>
        public List<Observation> Observations { get; set; }

        /// <summary>
        /// Returns a string representation of the object
        /// </summary>
        /// <returns>The object's string representation</returns>
        public override string ToString()
        {
            if (this.GetType() == typeof(HipObject))
            {
                return this.Name != null
                    ? $"HIP {this.CatalogueIdentifier} | {this.Name}"
                    : $"HIP {this.CatalogueIdentifier}";
            }

            if (this.GetType() == typeof(NgcObject))
            {
                return this.Name != null
                    ? $"NGC {this.CatalogueIdentifier} | {this.Name}"
                    : $"NGC {this.CatalogueIdentifier}";
            }

            return typeof(AstronomicalObject).ToString();
        }
    }
}
