namespace AstroCue.Server.Services.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for <see cref="ReportService"/>
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Generate reports for all observations in the database
        /// </summary>
        Task GenerateReports();
    }
}