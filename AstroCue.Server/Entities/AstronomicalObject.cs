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
        /// Gets or sets the name for the object (i.e. Polaris, Betelgeuse, Box Nebula etc...). As only a tiny fraction of objects
        /// have officially recognised names, this prop will be the catalogue + identifier in most cases (i.e.
        /// HIP xxxxx, NGC xxxxx etc...)
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a boolean representing whether or not the object is officially named
        /// </summary>
        [Required]
        public bool OfficiallyNamed { get; set; }

        /// <summary>
        /// Gets or sets the type of the object  
        /// </summary>
        [Required]
        public string Type { get; set; }

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
                return $"HIP {this.CatalogueIdentifier}";
            }

            if (this.GetType() == typeof(NgcObject))
            {
                return $"NGC {this.CatalogueIdentifier}";
            }

            return string.Empty;
        }
    }
}
