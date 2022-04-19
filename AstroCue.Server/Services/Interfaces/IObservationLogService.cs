namespace AstroCue.Server.Services.Interfaces
{
    using System.Collections.Generic;
    using Models.API.Inbound;
    using Models.API.Outbound;

    /// <summary>
    /// Interface for <see cref="ObservationLogService"/>
    /// </summary>
    public interface IObservationLogService
    {
        /// <summary>
        /// Add a new observation log to a user's account
        /// </summary>
        /// <param name="reqUserId">The ID of the user who made the request</param>
        /// <param name="model">An instance of <see cref="InboundObservationLogModel"/></param>
        /// <returns>An instance of <see cref="OutboundObservationLogModel"/></returns>
        OutboundObservationLogModel NewObservationLog(
            int reqUserId, 
            InboundObservationLogModel model);

        /// <summary>
        /// Retrieve all of the logs that belong to a given user, ordered by the date
        /// they were created
        /// </summary>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <returns>A list of <see cref="OutboundObservationLogModel"/> instances</returns>
        IList<OutboundObservationLogModel> GetAll(int reqUserId);
    }
}