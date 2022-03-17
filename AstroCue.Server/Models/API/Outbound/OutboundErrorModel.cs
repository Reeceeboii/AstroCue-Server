namespace AstroCue.Server.Models.API.Outbound
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Model representing an error returned from the API
    /// </summary>
    public class OutboundErrorModel
    {
        /// <summary>
        /// Gets or sets the error message
        /// </summary>
        [Required]
        public string Message { get; set; }
    }
}
