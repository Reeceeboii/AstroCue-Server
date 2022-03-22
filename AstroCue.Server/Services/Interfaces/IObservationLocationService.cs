namespace AstroCue.Server.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;
    using Models.API.Inbound;
    using Models.API.Outbound;

    /// <summary>
    /// Interface for <see cref="ObservationLocationService"/>
    /// </summary>
    public interface IObservationLocationService
    {
        /// <summary>
        /// Add a new observation location to a user's account
        /// </summary>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <param name="inboundModel">An instance of <see cref="InboundObsLocationModel"/></param>
        /// <returns>An instance of <see cref="OutboundObsLocationModel"/></returns>
        OutboundObsLocationModel AddNew(int reqUserId, InboundObsLocationModel inboundModel);

        /// <summary>
        /// Delete an observation location from a user's account
        /// </summary>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <param name="locationId">The ID of the <see cref="ObservationLocation"/> being deleted</param>
        /// <returns>An instance of <see cref="OutboundObsLocationModel"/>, or null</returns>
        OutboundObsLocationModel Delete(int reqUserId, int locationId);

        /// <summary>
        /// Retrieve all of the observation locations for a given user
        /// </summary>
        /// <param name="reqUserId">The ID of the user</param>
        /// <returns>A list of <see cref="OutboundObsLocationModel"/> instances</returns>
        Task<IList<OutboundObsLocationModel>> GetAllAsync(int reqUserId);

        /// <summary>
        /// Gets a static map for a specific <see cref="ObservationLocation"/>
        /// </summary>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <param name="locationId">The ID of the <see cref="ObservationLocation"/> that the map is being retrieved for</param>
        /// <returns>A byte array representing the image data</returns>
        /// <exception cref="ArgumentException">If the user does not an <see cref="ObservationLocation"/> matching
        /// the given ID</exception>
        Task<byte[]> GetStaticMapAsync(int reqUserId, int locationId);
    }
}