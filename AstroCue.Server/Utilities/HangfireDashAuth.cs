namespace AstroCue.Server.Utilities
{
    using Hangfire.Dashboard;

    /// <summary>
    /// Allows blanket access to Hangfire dashboard
    /// </summary>
    public class HangfireDashAuth : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// Authorises access to Hangfire's dashboard webpage
        /// </summary>
        /// <param name="context">Instance of <see cref="DashboardContext"/></param>
        /// <returns>A bool representing whether or not authorisation was granted</returns>
        public bool Authorize(DashboardContext context) => true;
    }
}
