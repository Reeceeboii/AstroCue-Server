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

        /// <summary>
        /// Set up a new observation between a location and an astronomical object
        /// </summary>
        /// <param name="locationId">The ID of the location where the observation will take place</param>
        /// <param name="astronomicalObjectId">The ID of the astronomical object that is being observed</param>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <returns>An instance of <see cref="OutboundObservationModel"/></returns>
        OutboundObservationModel NewObservation(int locationId, int astronomicalObjectId, int reqUserId);
    }
}