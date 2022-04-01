namespace AstroCue.Server.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.API.Outbound;

    /// <summary>
    /// Interface for <see cref="ReportService"/>
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Main crux of the AstroCue platform - generates astronomical observation reports,
        /// persists them to the database and emails them to users
        /// </summary>
        /// <param name="userIdForceGenerate">An ID given if the report generation process was kicked off manually
        /// by a user. Defaulted to 0 for all other cases</param>
        /// <returns><see cref="Task"/></returns>
        /// <exception cref="Exception">If the report generation process was started manually but the user
        /// who invoked it has no observations set up on their account</exception>
        Task GenerateReports(int userIdForceGenerate);

        /// <summary>
        /// Retrieve all of the reports that belong to a given user
        /// </summary>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <returns>A list of <see cref="OutboundReportModel"/> instances</returns>
        IList<OutboundReportModel> GetReports(int reqUserId);
    }
}