namespace AstroCue.Server.Services.Interfaces
{
    using Entities;

    /// <summary>
    /// Interface for <see cref="EmailService"/>
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send an email to a newly signed up user welcoming them to AstroCue
        /// </summary>
        /// <param name="user">An instance of <see cref="AstroCueUser"/></param>
        void SendWelcomeEmail(AstroCueUser user);
    }
}