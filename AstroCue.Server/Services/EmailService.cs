namespace AstroCue.Server.Services
{
    using System;
    using Entities;
    using Interfaces;

    /// <summary>
    /// Service class for handling email communications
    /// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        /// Send an email to a newly signed up user welcoming them to AstroCue
        /// </summary>
        /// <param name="user">An instance of <see cref="AstroCueUser"/></param>
        public async void SendWelcomeEmail(AstroCueUser user)
        {
            throw new NotImplementedException();
        }
    }
}
