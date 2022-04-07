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
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <returns>A list of <see cref="OutboundAstronomicalObjectModel"/> instances
        /// that match the search queries</returns>
        IList<OutboundAstronomicalObjectModel> ObjectSearch(AstronomicalObjectSearchParams searchParams, int reqUserId);

        /// <summary>
        /// Set up a new observation between a location and an astronomical object
        /// </summary>
        /// <param name="locationId">The ID of the location where the observation will take place</param>
        /// <param name="astronomicalObjectId">The ID of the astronomical object that is being observed</param>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <returns>An instance of <see cref="OutboundObservationModel"/></returns>
        OutboundObservationModel NewObservation(int locationId, int astronomicalObjectId, int reqUserId);

        /// <summary>
        /// Retrieve all of the observations belonging to a certain user
        /// </summary>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <returns>A list of <see cref="OutboundObservationModel"/> instances</returns>
        IList<OutboundObservationModel> GetAll(int reqUserId);

        /// <summary>
        /// Delete an existing observation from an account
        /// </summary>
        /// <param name="observationId">The ID of the observation to delete</param>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <returns>An instance of <see cref="OutboundObservationModel"/></returns>
        OutboundObservationModel DeleteObservation(int observationId, int reqUserId);
    }
}