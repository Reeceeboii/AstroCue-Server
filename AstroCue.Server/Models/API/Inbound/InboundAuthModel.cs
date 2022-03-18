namespace AstroCue.Server.Models.API.Inbound
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Model class reresenting an incoming set of authentication credentials
    /// </summary>
    public class InboundAuthModel
    {
        /// <summary>
        /// Gets or sets the user's email address
        /// </summary>
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the user's password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
