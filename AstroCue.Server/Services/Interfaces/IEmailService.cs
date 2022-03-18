namespace AstroCue.Server.Services.Interfaces
{
    using System.Threading.Tasks;
    using Entities;
    using RestSharp;

    /// <summary>
    /// Interface for <see cref="EmailService"/>
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send an email to a newly signed up user welcoming them to AstroCue
        /// </summary>
        /// <param name="user">An instance of <see cref="AstroCueUser"/></param>
        Task<RestResponse> SendWelcomeEmail(AstroCueUser user);
    }
}