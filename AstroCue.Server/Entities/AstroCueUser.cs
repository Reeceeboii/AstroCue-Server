namespace AstroCue.Server.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Entity class used to represent an AstroCue user
    /// </summary>
    public class AstroCueUser
    {
        /// <summary>
        /// Gets or sets the user's ID
        /// </summary>
        public int Id { get; set; }

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
        /// Gets or sets a list of observation locations owned by the user
        /// </summary>
        public List<ObservationLocation> ObservationLocations { get; set; }

        /// <summary>
        /// Gets or sets a list of observation reports that have been generated for the user
        /// </summary>
        public List<Report> Reports { get; set; }

        /// <summary>
        /// Gets or sets a list of observation logs that the user has created
        /// </summary>
        public List<ObservationLog> ObservationLogs { get; set; }

        /// <summary>
        /// Gets or sets the user's password hash as an array of bytes
        /// </summary>
        [Required]
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the user's passwordl salt as an array of bytes
        /// </summary>
        [Required]
        public byte[] PasswordSalt { get; set; }
    }
}
