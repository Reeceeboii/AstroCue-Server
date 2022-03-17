namespace AstroCue.Server.Services
{
    using Data;
    using Entities;
    using Interfaces;

    /// <summary>
    /// Service class to manage AstroCue users
    /// </summary>
    public class AstroCueUserService : IAstroCueUserService
    {
        /// <summary>
        /// Instance of <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initialises a new instance of the <see cref="AstroCueUserService"/> class
        /// </summary>
        /// <param name="context">Instance of <see cref="ApplicationDbContext"/></param>
        public AstroCueUserService(ApplicationDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Retieves an <see cref="AstroCueUser"/> from the database context using an Id
        /// </summary>
        /// <param name="id">A user Id</param>
        /// <returns>An instance of <see cref="AstroCueUser"/> if a matching record is found, or returns null</returns>
        public AstroCueUser RetrieveById(int id)
        {
            return this._context.AstroCueUsers.Find(id);
        }
    }
}
