namespace AstroCue.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models.API.Outbound;
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
        private readonly IReportService reportService;

        /// <summary>
        /// Initialises a new instance of the <see cref="ReportController"/> class
        /// </summary>
        /// <param name="reportService">Instance of <see cref="IReportService"/></param>
        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        /// <summary>
        /// Retrieve all of the reports for a given user
        /// </summary>
        /// <returns>A list of <see cref="OutboundReportModel"/> instances</returns>
        [HttpGet]
        [Route("all")]
        [SwaggerOperation(
            Summary = "Retrieve all reports",
            Description = "Retrieve all reports for a given user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", typeof(IList<OutboundReportModel>))]
        public IActionResult GetAllReports()
        {
            int reqUserId = (int)this.HttpContext.Items[Constants.HttpContextReqUserId]!;
            return this.Ok(this.reportService.GetReports(reqUserId));
        }

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
                await this.reportService.GenerateReports(reqUserId);
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

    }
}
