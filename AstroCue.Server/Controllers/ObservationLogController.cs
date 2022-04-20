namespace AstroCue.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models.API.Inbound;
    using Models.API.Outbound;
    using Parameters;
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

        /// <summary>
        /// Endpoint that allows users to create new observation logs against reports
        /// that astrocue has created for them
        /// </summary>
        /// <param name="model">An instance of <see cref="InboundObservationLogModel"/></param>
        /// <returns><see cref="IActionResult"/></returns>
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
                return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel
                {
                    Message = exc.Message
                });
            }

            if (log == null)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new OutboundErrorModel
                {
                    Message = "Internal server error"
                });
            }

            return this.Ok(log);
        }

        /// <summary>
        /// Endpoint that allows users to retrieve all of the reports they have created
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpGet]
        [Route("all")]
        [SwaggerOperation(
            Summary = "Get all logs",
            Description = "Retrieve all of the observation logs that are linked to an account")]
        [SwaggerResponse(
            StatusCodes.Status200OK, "The request completed successfully", typeof(IList<OutboundObservationLogModel>))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Something went wrong, check the request parameters",
            typeof(OutboundErrorModel))]
        public IActionResult GetAll()
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;
            return this.Ok(this._observationLogService.GetAll(reqUserId));
        }

        [HttpDelete]
        [Route("delete")]
        [SwaggerOperation(
            Summary = "Delete an observation log",
            Description = "Remove an observation log from an account by providing its ID")]
        [SwaggerResponse(
            StatusCodes.Status200OK, 
            "The request completed successfully",
            typeof(OutboundObservationLogModel))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest, 
            "Something went wrong, check the request parameters",
            typeof(OutboundErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status500InternalServerError,
            "Internal server error",
            typeof(OutboundErrorModel))]
        public IActionResult DeleteObservationLog(
            [FromQuery, SwaggerParameter("ID of observation log to delete", Required = true)]
            IdParameter idParameter)
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;

            OutboundObservationLogModel log;

            try
            {
                log = this._observationLogService.Delete(reqUserId, idParameter);
            }
            catch (Exception exc)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel
                {
                    Message = exc.Message
                });
            }

            if (log == null)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new OutboundErrorModel
                {
                    Message = "Internal server error"
                });
            }

            return this.Ok(log);
        }

        /// <summary>
        /// Endpoint that allows users to edit an existing observation log on their account
        /// </summary>
        /// <param name="model">An instance of <see cref="InboundObservationLogEditModel"/></param>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpPatch]
        [Route("edit")]
        [SwaggerOperation(
            Summary = "Edit an observation log",
            Description = "Provide a target ID and a new set of details to edit an observation log")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "The request completed successfully",
            typeof(OutboundObservationLogModel))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Something went wrong, check the request parameters",
            typeof(OutboundErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status500InternalServerError,
            "Internal server error",
            typeof(OutboundErrorModel))]
        public IActionResult EditObservationLog([FromBody] InboundObservationLogEditModel model)
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;

            OutboundObservationLogModel log;

            try
            {
                log = this._observationLogService.Edit(reqUserId, model);
            }
            catch (Exception exc)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel
                {
                    Message = exc.Message
                });
            }

            if (log == null)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new OutboundErrorModel
                {
                    Message = "Internal server error"
                });
            }

            return this.Ok(log);
        }
    }
}
