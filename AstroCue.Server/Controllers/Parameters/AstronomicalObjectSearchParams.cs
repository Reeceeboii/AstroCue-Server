namespace AstroCue.Server.Controllers.Parameters
{
    using System.ComponentModel.DataAnnotations;
    using Enums;

    /// <summary>
    /// Query parameter binding for any endpoint that requires a plain
    /// </summary>
    public class AstronomicalObjectSearchParams
    {
        /// <summary>
        /// Gets or sets a string search query
        /// </summary>
        [Required]
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets a limit on the number of results returned
        /// </summary>
        [Required]
        [Range(1, 10, ErrorMessage = "Limit needs to be between 1 and 10")]
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets an optional popular object filter
        /// </summary>
        public bool Popular { get; set; }

        /// <summary>
        /// Gets or sets an optional object type filter.
        /// Can be any of <see cref="AstronomicalObjectTypeEnum"/>
        /// </summary>
        [Required]
        [EnumDataType(typeof(AstronomicalObjectTypeEnum))]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets an optional location filter
        /// (used for location-based visibility filtering)
        /// </summary>
        public int LocationId { get; set; }
    }
}
