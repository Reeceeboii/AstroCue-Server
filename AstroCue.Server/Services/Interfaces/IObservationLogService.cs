namespace AstroCue.Server.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Controllers.Parameters;
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

        /// <summary>
        /// Deletes a single log from a user's account
        /// </summary>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <param name="idParam">An instance of <see cref="IdParameter"/></param>
        /// <returns>An instance of <see cref="OutboundObservationLogModel"/></returns>
        /// <exception cref="Exception">If the log does not exist on the account</exception>
        OutboundObservationLogModel Delete(int reqUserId, IdParameter idParam);
    }
}