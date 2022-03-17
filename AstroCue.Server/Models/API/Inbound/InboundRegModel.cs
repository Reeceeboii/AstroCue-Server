namespace AstroCue.Server.Models.API.Inbound
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Model to represent a new user registration attempt
    /// </summary>
    public class InboundRegModel
    {
        /// <summary>
        /// Gets or sets the user's email address
        /// </summary>
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the user's first name
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
