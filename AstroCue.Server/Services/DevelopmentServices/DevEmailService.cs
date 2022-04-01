namespace AstroCue.Server.Services.DevelopmentServices
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces;
    using Models.Email.Reports;
    using RestSharp;

    /// <summary>
    /// Class to handle email related operations during development. Rather than flooding an inbox,
    /// this service will simply output the intent of the operation and its data. No API contact
    /// is attempted.
    /// </summary>
    public class DevEmailService : IEmailService
    {
        /// <summary>
        /// Development version of <see cref="IEmailService.SendWelcomeEmail"/>
        /// </summary>
        /// <param name="user">An instance of <see cref="AstroCueUser"/></param>
        /// <returns>Completed <see cref="Task"/></returns>
        public Task<RestResponse> SendWelcomeEmail(AstroCueUser user)
        {
            Debug.WriteLine("Welcome email sent");
            RestResponse resp = new(); 
            TaskCompletionSource<RestResponse> src = new();
            src.SetResult(resp);
            return src.Task;
        }

        /// <summary>
        /// Development version of <see cref="IEmailService.SendReportEmail"/>
        /// </summary>
        /// <param name="user">An instance of <see cref="AstroCueUser"/></param>
        /// <param name="reportList">A list of <see cref="LocationReport"/> instances</param>
        /// <param name="staticMaps">Static map image mappings to their filenames</param>
        /// <returns>Completed <see cref="Task"/></returns>
        public Task<RestResponse> SendReportEmail(AstroCueUser user, List<LocationReport> reportList, Dictionary<string, byte[]> staticMaps)
        {
            Debug.WriteLine("Report email sent");
            RestResponse resp = new();
            TaskCompletionSource<RestResponse> src = new();
            src.SetResult(resp);
            return src.Task;
        }
    }
}
