namespace AstroCue.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Entities;
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
    /// Controller class to handle observation location based operations
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ObservationLocationController : ControllerBase
    {
        /// <summary>
        /// Instance of <see cref="IObservationLocationService"/>
        /// </summary>
        private readonly IObservationLocationService _observationLocationService;

        /// <summary>
        /// Instance of <see cref="IMapper"/>
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="ObservationLocationController"/> class
        /// </summary>
        /// <param name="observationLocationService">Instance of <see cref="IObservationLocationService"/></param>
        /// <param name="mapper">Instance of <see cref="IMapper"/></param>
        public ObservationLocationController(
            IObservationLocationService observationLocationService,
            IMapper mapper)
        {
            this._observationLocationService = observationLocationService;
            this._mapper = mapper;
        }

        /// <summary>
        /// Add a new observation location to a user's account
        /// </summary>
        /// <param name="inboundObsLocationModel">An instance of <see cref="InboundObsLocationModel"/></param>
        /// <returns>An instance of <see cref="OutboundObsLocationModel"/> if the request was successful, or if not,
        /// an instance of <see cref="OutboundErrorModel"/></returns>
        [HttpPost]
        [Route("new")]
        [SwaggerOperation(
            Summary = "Add a new observation location to an account",
            Description = "Add a new observation location to an account by providing its details")]
        [SwaggerResponse(StatusCodes.Status200OK,
            "The request completed successfully and the observation location has been added to the account",
            typeof(OutboundObsLocationModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "There was an error adding the location", typeof(OutboundErrorModel))]
        public IActionResult AddNewObservationLocation(
            [FromBody, SwaggerParameter("Observation location", Required = true)] InboundObsLocationModel inboundObsLocationModel)
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;

            OutboundObsLocationModel loc = null;

            try
            {
                loc = this._observationLocationService.AddNew(reqUserId, inboundObsLocationModel);
            }
            catch (Exception exc)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel
                {
                    Message = exc.Message
                });
            }

            if (loc == null)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new OutboundErrorModel
                {
                    Message = "Failed to add location to your account, please try again"
                });
            }

            return this.Ok(loc);
        }

        /// <summary>
        /// Endpoint that allows existing observation locations to have their details edited
        /// </summary>
        /// <param name="inboundObsLocationModel">Instance of <see cref="InboundObsLocationModel"/></param>
        /// <param name="idParameter">Instance of <see cref="IdParameter"/></param>
        /// <returns>An instance of <see cref="OutboundObsLocationModel"/> if the request was successful, or if not,
        /// an instance of <see cref="OutboundErrorModel"/></returns>
        [HttpPatch]
        [Route("edit")]
        [SwaggerOperation(
            Summary = "Edit an observation location",
            Description = "Provide a set of updated observation details to update an existing location with")]
        [SwaggerResponse(StatusCodes.Status200OK,
            "The request completed successfully and the observation location has been updated",
            typeof(OutboundObsLocationModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            "Check parameters", typeof(OutboundErrorModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError,
            "Internal server error", typeof(OutboundErrorModel))]
        public IActionResult EditObservationLocation(
            [FromBody, SwaggerParameter("Observation location", Required = true)]
            InboundObsLocationModel inboundObsLocationModel,
            [FromQuery, SwaggerParameter("ID of location to edit", Required = true)]
            IdParameter idParameter)
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;

            OutboundObsLocationModel loc = null;
            try
            {
                loc = this._observationLocationService.Edit(reqUserId, idParameter.Id, inboundObsLocationModel);
            }
            catch (Exception exc)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel
                {
                    Message = exc.Message
                });
            }

            if (loc == null)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new OutboundErrorModel
                {
                    Message = "Failed to apply edits, please try again"
                });
            }

            return this.Ok(loc);
        }

        /// <summary>
        /// Deletes an observation location from a user's account
        /// </summary>
        /// <param name="idParam">An instance of <see cref="IdParameter"/></param>
        /// <returns>An instance of <see cref="OutboundObsLocationModel"/></returns>
        [HttpDelete]
        [Route("delete")]
        [SwaggerOperation(
            Summary = "Delete an observation location from an account",
            Description = "Provide an ID for an observation location and have it deleted from an account")]
        [SwaggerResponse(StatusCodes.Status200OK,
            "The request completed successfully and the observation location has been deleted from the account")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Error with parameters or deletion")]
        public IActionResult DeleteObservationLocation(
            [FromQuery, SwaggerParameter("Id", Required = true)] IdParameter idParam)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;

            OutboundObsLocationModel loc;
            try
            {
                loc = this._observationLocationService.Delete(reqUserId, idParam.Id);
            }
            catch (ArgumentException exc)
            {
                return this.BadRequest(new OutboundErrorModel()
                {
                    Message = exc.Message
                });
            }

            if (loc == null)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new OutboundErrorModel()
                {
                    Message = "Server error, please try again"
                });
            }

            return this.Ok(loc);
        }

        /// <summary>
        /// Get all of the observation locations attached to a given account
        /// </summary>
        /// <returns>A list of <see cref="OutboundObsLocationModel"/> instances</returns>
        [HttpGet]
        [Route("all")]
        [SwaggerOperation(
            Summary = "Get all observation locations",
            Description = "Retrieves all of the observation locations for a given account")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request was successful", typeof(List<OutboundObsLocationModel>))]
        public async Task<ActionResult<List<OutboundObsLocationModel>>> GetAllObservationLocations()
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;
            return this.Ok(await this._observationLocationService.GetAllAsync(reqUserId));
        }

        /// <summary>
        /// Given the ID of an <see cref="ObservationLocation"/>
        /// </summary>
        /// <param name="locationId">The ID of the location to retrieve the static map for</param>
        /// <param name="asBase64">Boolean representing whether or not the image should be sent back in base64 format</param>
        /// <returns>A static map image from the MapBox API</returns>
        [HttpGet]
        [Route("static-map/{locationId:int}")]
        [SwaggerOperation(
            Summary = "Static map images",
            Description = "Given the ID of an observation location, return a static map for it")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request was successful")]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            "The location does not exist on the authenticated account",
            typeof(OutboundErrorModel))]
        public async Task<IActionResult> GetStaticMap(
            int locationId,
            [FromQuery, SwaggerParameter(Description = "Retrieve as base 64", Required = false)] bool asBase64)
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;

            if (!asBase64)
            {
                byte[] imageBytes;

                try
                {
                    imageBytes = await this._observationLocationService.GetStaticMapAsync(reqUserId, locationId);
                }
                catch (ArgumentException exc)
                {
                    return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel
                    {
                        Message = exc.Message
                    });
                }
                return this.File(imageBytes, "image/png");
            }

            string imageBase64;

            try
            {
                imageBase64 = await this._observationLocationService.GetStaticMapAsBase64Async(reqUserId, locationId);
            }
            catch (ArgumentException exc)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel
                {
                    Message = exc.Message
                });
            }

            return this.Ok(imageBase64);

        }
    }
}
