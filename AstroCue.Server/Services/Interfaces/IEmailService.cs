namespace AstroCue.Server.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;
    using Models.Email.Reports;
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
        /// <returns><see cref="RestResponse"/></returns>
        Task<RestResponse> SendWelcomeEmail(AstroCueUser user);

        /// <summary>
        /// Send a report email to a user
        /// </summary>
        /// <param name="user">An instance of <see cref="AstroCueUser"/></param>
        /// <param name="reportList">A list of <see cref="LocationReport"/> instances</param>
        /// <param name="staticMaps">Static map image mappings to their filenames</param>
        /// <returns><see cref="RestResponse"/></returns>
        Task<RestResponse> SendReportEmail(AstroCueUser user, List<LocationReport> reportList,
            Dictionary<string, byte[]> staticMaps);
    }
}