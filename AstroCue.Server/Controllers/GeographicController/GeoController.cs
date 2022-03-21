namespace AstroCue.Server.Controllers.GeographicController
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models.API.Outbound;
    using Models.Misc;
    using Services.Interfaces;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller for handling geographic operations (distinctly separate from
    /// observation location operations). This is separate due to the inclusion of
    /// the light pollution service being a part of the observation location controller.
    ///
    /// Initialising GDAL every single time a forward geocode request comes in is an extremely
    /// large waste of resources; separating those calls out to their own controller removes this overhead.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GeoController : ControllerBase
    {
        /// <summary>
        /// Instance of <see cref="IMappingService"/>
        /// </summary>
        private readonly IMappingService _mappingService;

        /// <summary>
        /// Initialises a new instance of the <see cref="GeoController"/> class
        /// </summary>
        /// <param name="mappingService">Instance of <see cref="IMappingService"/></param>
        public GeoController(IMappingService mappingService)
        {
            this._mappingService = mappingService;
        }

        /// <summary>
        /// Forward geocoding endpoint - allows searching for locations via text queries
        /// </summary>
        /// <param name="parameters">A query to forward to the MapBox API</param>
        /// <returns>A list of <see cref="FwdGeocodeResult"/> instances</returns>
        [HttpGet]
        [Route("search")]
        [SwaggerOperation(
            Summary = "Forward geocode",
            Description = "Takes a string query and attempts to forward geocode it to a lat/lon pair")]
        [SwaggerResponse(StatusCodes.Status200OK, "The request completed successfully", typeof(IList<FwdGeocodeResult>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "There was something wrong with the request. Check the parameters.", typeof(OutboundErrorModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(OutboundErrorModel))]
        public async Task<IActionResult> ForwardGeocode(
            [FromQuery, SwaggerParameter("Search query", Required = true)] FwdGeocodeParams parameters)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            IList<FwdGeocodeResult> features;

            try
            {
                features = await this._mappingService.ForwardGeocodeAsync(parameters.Query);
            }
            catch (ArgumentOutOfRangeException exc)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel
                {
                    Message = exc.Message
                });
            }
            catch (ArgumentException exc)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel
                {
                    Message = exc.Message
                });
            }
            catch (Exception exc)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new OutboundErrorModel
                {
                    Message = exc.Message
                });
            }

            return this.Ok(features);
        }
    }
}
