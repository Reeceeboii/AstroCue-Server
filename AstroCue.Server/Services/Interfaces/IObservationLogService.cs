namespace AstroCue.Server.Services.Interfaces
{
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
    }
}