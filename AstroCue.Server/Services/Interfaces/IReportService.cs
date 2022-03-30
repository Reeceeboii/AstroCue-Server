namespace AstroCue.Server.Services.Interfaces
{
    /// <summary>
    /// Interface for <see cref="ReportService"/>
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Generate reports for all observations in the database
        /// </summary>
        void GenerateReports();
    }
}