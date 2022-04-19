namespace AstroCue.Server.Controllers
{
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models.API.Inbound;
    using Models.API.Outbound;
    using Services.Interfaces;
    using Swashbuckle.AspNetCore.Annotations;
    using Utilities;

    /// <summary>
    /// Controller class to handle astronomical log operations
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ObservationLogController : ControllerBase
    {
        /// <summary>
        /// Instance of <see cref="IObservationLogService"/>
        /// </summary>
        private readonly IObservationLogService _observationLogService;

        /// <summary>
        /// Initialises a new instance of <see cref="ObservationLogController"/>
        /// </summary>
        /// <param name="observationLogService">Instance of <see cref="ObservationLogController"/></param>
        public ObservationLogController(IObservationLogService observationLogService)
        {
            this._observationLogService = observationLogService;
        }

        [HttpPost]
        [Route("new")]
        [SwaggerOperation(
            Summary = "Create a new observation log",
            Description = "Add a new observation log to a user's account - linked to a report")]
        [SwaggerResponse(
            StatusCodes.Status200OK, "The request completed successfully", typeof(OutboundObservationLogModel))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Something went wrong, check the request parameters",
            typeof(OutboundErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status500InternalServerError,
            "Internal server error",
            typeof(OutboundErrorModel))]
        public IActionResult NewLog([FromBody] InboundObservationLogModel model)
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;

            OutboundObservationLogModel log;

            try
            {
                log = this._observationLogService.NewObservationLog(reqUserId, model);
            }
            catch (Exception exc)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel()
                {
                    Message = exc.Message
                });
            }

            if (log == null)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new OutboundErrorModel()
                {
                    Message = "Internal server error"
                });
            }

            return this.Ok(log);
        }
    }
}
