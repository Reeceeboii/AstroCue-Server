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
    /// Controller class to handle astronomical observation operations
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ObservationController : ControllerBase
    {
        /// <summary>
        /// Instance of <see cref="IObservationService"/>
        /// </summary>
        readonly IObservationService _observationService;
        
        /// <summary>
        /// Initialises a new instance of the <see cref="ObservationController"/> class
        /// </summary>
        /// <param name="observationService">Instance of <see cref="IObservationService"/></param>
        public ObservationController(IObservationService observationService)
        {
            this._observationService = observationService;
        }

        /// <summary>
        /// Endpoint for searching for astronomical objects
        /// </summary>
        /// <param name="searchParams">An instance of <see cref="AstronomicalObjectSearchParams"/></param>
        /// <returns>A list of <see cref="OutboundAstronomialObjectModel"/>, or an instance of
        /// <see cref="OutboundErrorModel"/> in the case of an error</returns>
        [HttpGet]
        [Route("object-search")]
        [SwaggerOperation(
            Summary = "Astronomical object search",
            Description = "Search for astronomical objects using a mixture of queries and flags")]
        [SwaggerResponse(
            StatusCodes.Status200OK, 
            "Search was successful", typeof(IList<OutboundAstronomialObjectModel>))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "There was an issue with the query parameters", typeof(OutboundErrorModel))]
        public IActionResult ObjectSearch(
            [FromQuery, SwaggerParameter("Query operations")] AstronomicalObjectSearchParams searchParams)
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;

            IList<OutboundAstronomialObjectModel> objects;

            try
            {
                objects = this._observationService.ObjectSearch(searchParams, reqUserId);
            }
            catch (ArgumentException exc)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel()
                {
                    Message = exc.Message
                });
            }

            return this.Ok(objects);
        }

        /// <summary>
        /// Endpoint for adding a new astronomical observation to the system
        /// </summary>
        /// <param name="observationModel">An instance of <see cref="InboundObservationModel"/></param>
        /// <returns>An instance of <see cref="OutboundObservationModel"/>, or an instance of
        /// <see cref="OutboundErrorModel"/> in the case of an error</returns>
        [HttpPost]
        [Route("new")]
        [SwaggerOperation(
            Summary = "New astronomical observation",
            Description =
                "Creates a new observation using the ID of an observation location and an astronomical object")]
        [SwaggerResponse(StatusCodes.Status200OK, "Observation created", typeof(OutboundObservationModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(OutboundErrorModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(OutboundErrorModel))]
        public IActionResult NewObservation(
            [FromBody, SwaggerParameter("Request parameters", Required = true)] InboundObservationModel observationModel)
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;

            OutboundObservationModel observation;

            try
            {
                observation = this._observationService.NewObservation(
                    observationModel.LocationId,
                    observationModel.AstronomicalObjectId,
                    reqUserId);
            }
            catch(ArgumentException exc)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel()
                {
                    Message = exc.Message
                });
            }

            if (observation == null)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new OutboundErrorModel()
                {
                    Message = "Internal error, please try again!"
                });
            }

            return this.Ok(observation);
        }

        /// <summary>
        /// Endpoint to retrieve all of the observations that a user has created
        /// </summary>
        /// <returns>An instance of <see cref="OutboundObservationModel"/>, or an instance of
        /// <see cref="OutboundErrorModel"/> in the case of an error</returns>
        [HttpGet]
        [Route("all")]
        [SwaggerOperation(
            Summary = "Get all observations",
            Description = "Get all of the astronomical observations attached to an account")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", typeof(IList<OutboundObservationModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(OutboundErrorModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(OutboundErrorModel))]
        public IActionResult GetAllObservations()
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;
            return this.Ok(this._observationService.GetAll(reqUserId));
        }

        /// <summary>
        /// Endpoint to delete an observation from a user's account
        /// </summary>
        /// <param name="idParameter">An instance of <see cref="IdParameter"/></param>
        /// <returns>An instance of <see cref="OutboundObservationModel"/>, or an instance of
        /// <see cref="OutboundErrorModel"/> in the case of an error</returns>
        [HttpDelete]
        [Route("delete")]
        [SwaggerOperation(
            Summary = "Delete an astronomical observation",
            Description = "Delete an astronomical observation from an account by providing its ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Observation deleted", typeof(OutboundObservationModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(OutboundErrorModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(OutboundErrorModel))]
        public IActionResult DeleteObservation(
            [FromQuery, SwaggerParameter("The ID of the location to delete", Required = true)]
            IdParameter idParameter)
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;

            OutboundObservationModel observation = null;

            try
            {
                observation = this._observationService.DeleteObservation(idParameter.Id, reqUserId);
            }
            catch (ArgumentException exc)
            {
                this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel()
                {
                    Message = exc.Message
                });
            }

            if (observation == null)
            {
                this.StatusCode(StatusCodes.Status500InternalServerError, new OutboundErrorModel()
                {
                    Message = "Internal error, please try again!"
                });
            }

            return this.Ok(observation);
        }
    }
}
