namespace AstroCue.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models.API.Outbound;
    using Parameters;
    using Services.Interfaces;
    using Swashbuckle.AspNetCore.Annotations;
    using Utilities;

    /// <summary>
    /// Controller to handle report based operations
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        /// <summary>
        /// Instance of <see cref="IReportService"/>
        /// </summary>
        private readonly IReportService _reportService;

        /// <summary>
        /// Initialises a new instance of the <see cref="ReportController"/> class
        /// </summary>
        /// <param name="reportService">Instance of <see cref="IReportService"/></param>
        public ReportController(IReportService reportService)
        {
            this._reportService = reportService;
        }

        /// <summary>
        /// Retrieve all of the reports for a given user
        /// </summary>
        /// <returns>A list of <see cref="OutboundObsLocReportModel"/> instances</returns>
        [HttpGet]
        [Route("all")]
        [SwaggerOperation(
            Summary = "Retrieve all reports",
            Description = "Retrieve all reports for a given user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", typeof(IList<OutboundObsLocReportModel>))]
        public IActionResult GetAllReports()
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;
            return this.Ok(this._reportService.GetReports(reqUserId));
        }

        /// <summary>
        /// Endpoint that allows users to force generation of reports for their observations
        /// </summary>
        /// <returns><see cref="StatusCodeResult"/> or an <see cref="OutboundErrorModel"/></returns>
        [HttpPost]
        [Route("force-generate")]
        [SwaggerOperation(
            Summary = "Generate a new set of reports",
            Description = "For each observation the user has, generate a report for each one and " +
                          "email the results to their registered email")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request failed", typeof(OutboundErrorModel))]
        public async Task<IActionResult> GenerateReports()
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;

            try
            {
                await this._reportService.GenerateReports(reqUserId);
            }
            catch (Exception exc)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel()
                {
                    Message = exc.Message
                });
            }

            return this.Ok(new
            {
                Message = "Email sent!"
            });
        }

        /// <summary>
        /// Delete a report from an account by providing its ID
        /// </summary>
        /// <param name="idParameter">Instance of <see cref="IdParameter"/></param>
        /// <returns>An <see cref="OutboundReportModel"/>, or an <see cref="OutboundErrorModel"/></returns>
        [HttpDelete]
        [Route("delete")]
        [SwaggerOperation(
            Summary = "Delete an report",
            Description = "Delete an observation from an account by providing its ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", typeof(OutboundReportModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Check parameters", typeof(OutboundErrorModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(OutboundErrorModel))]
        public IActionResult DeleteReport(
            [FromQuery, SwaggerParameter("ID", Required = true)]
            IdParameter idParameter)
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;

            OutboundReportModel report;

            try
            {
                report = this._reportService.DeleteReport(reqUserId, idParameter.Id);
            }
            catch (Exception exc)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, new OutboundErrorModel()
                {
                    Message = exc.Message
                });
            }

            if (report == null)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new OutboundErrorModel()
                {
                    Message = "There was an error deleting that report, please try again"
                });
            }

            return this.Ok(report);
        }

    }
}
