namespace AstroCue.Server.Services
{
    using Data;

    /// <summary>
    /// Service layer class to handle observation log operations
    /// </summary>
    public class ObservationLogService
    {
        /// <summary>
        /// Instance of <see cref="ApplicationDbContext"/>
        /// </summary>
        private ApplicationDbContext _context;

        /// <summary>
        /// Initialises a new instance of the <see cref="ObservationLogService"/> class
        /// </summary>
        /// <param name="context">Instance of <see cref="ApplicationDbContext"/></param>
        public ObservationLogService(ApplicationDbContext context)
        {
            this._context = context;
        }


    }
}
