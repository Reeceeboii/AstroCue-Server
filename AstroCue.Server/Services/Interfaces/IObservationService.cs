namespace AstroCue.Server.Services.Interfaces
{
    using System.Collections.Generic;
    using Controllers.Parameters;
    using Models.API.Outbound;

    /// <summary>
    /// Interface for <see cref="ObservationService"/>
    /// </summary>
    public interface IObservationService
    {
        /// <summary>
        /// Search the database for astronomical objects with a set of parameters
        /// </summary>
        /// <param name="searchParams">An instance of <see cref="AstronomicalObjectSearchParams"/></param>
        /// <param name="reqUserId">ID of the user that made the request</param>
        /// <returns>A list of <see cref="OutboundAstronomialObjectModel"/> instances
        /// that match the search queries</returns>
        IList<OutboundAstronomialObjectModel> ObjectSearch(AstronomicalObjectSearchParams searchParams, int reqUserId);
    }
}