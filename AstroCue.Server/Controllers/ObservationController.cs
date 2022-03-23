namespace AstroCue.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        [Route("object-search")]
        [SwaggerOperation(
            Summary = "Astronomical object search",
            Description = "Search for astronomical objects using a mixture of queries and flags")
        ]
        [SwaggerResponse(
            StatusCodes.Status200OK, 
            "Search was successful", typeof(IList<OutboundAstronomialObjectModel>))
        ]
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
    }
}
