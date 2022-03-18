namespace AstroCue.Server.Services.Interfaces
{
    using Entities;

    /// <summary>
    /// Interface for <see cref="AstroCueUserService"/>
    /// </summary>
    public interface IAstroCueUserService
    {
        /// <summary>
        /// Retieves an <see cref="AstroCueUser"/> from the database context using an Id
        /// </summary>
        /// <param name="id">A user Id</param>
        /// <returns>An instance of <see cref="AstroCueUser"/> if a matching record is found, or returns null</returns>
        AstroCueUser RetrieveById(int id);
    }
}