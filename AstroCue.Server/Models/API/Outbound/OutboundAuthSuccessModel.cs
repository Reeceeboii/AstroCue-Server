﻿namespace AstroCue.Server.Models.API.Outbound
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Model class representing a successful authentication
    /// </summary>
    public class OutboundAuthSuccessModel
    {
        /// <summary>
        /// Gets or sets the user's Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user's email address
        /// </summary>
        [EmailAddress]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the user's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the JWT that was created as a result of a successful authentication
        /// </summary>
        public string Token { get; set; }
    }
}
