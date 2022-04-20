namespace AstroCue.Server.Models.API.Inbound
{
    using System.ComponentModel.DataAnnotations;
    using Controllers.Parameters.Enums;

    /// <summary>
    /// Model class to represent a new incoming astronomical log
    /// </summary>
    public class InboundObservationLogModel
    {
        /// <summary>
        /// The ID of the report that this log is being created against.
        /// This report must exist on the account before the log is created, and
        /// this reference will be checked
        /// </summary>
        [Required]
        public int ReportId { get; set; }

        /// <summary>
        /// Gets or sets a textual description of what was observed
        /// </summary>
        [Required]
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Must be between 1 and 2000 characters")]
        public string TextualDescription { get; set; }

        /// <summary>
        /// Gets or sets the name(s) of those who carried out the observation
        /// </summary>
        [StringLength(250, ErrorMessage = "Must not exceed 250 characters")]
        public string Observer { get; set; }

        /// <summary>
        /// Gets or sets the type of observation that was carried out.
        /// Can be any of <see cref="ObservationTypeEnum"/>
        /// </summary>
        [Required]
        [EnumDataType(typeof(ObservationTypeEnum))]
        public string ObservationType { get; set; }
    }
}
