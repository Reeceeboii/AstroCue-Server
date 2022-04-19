﻿namespace AstroCue.Server.Services
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Data;
    using Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models.API.Inbound;
    using Models.API.Outbound;

    /// <summary>
    /// Service layer class to handle observation log operations
    /// </summary>
    public class ObservationLogService : IObservationLogService
    {
        /// <summary>
        /// Instance of <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Instance of <see cref="IMapper"/>
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="ObservationLogService"/> class
        /// </summary>
        /// <param name="context">Instance of <see cref="ApplicationDbContext"/></param>
        /// <param name="mapper">Instance of <see cref="IMapper"/></param>
        public ObservationLogService(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        /// <summary>
        /// Add a new observation log to a user's account
        /// </summary>
        /// <param name="reqUserId">The ID of the user who made the request</param>
        /// <param name="model">An instance of <see cref="InboundObservationLogModel"/></param>
        /// <returns>An instance of <see cref="OutboundObservationLogModel"/></returns>
        public OutboundObservationLogModel NewObservationLog(
            int reqUserId, 
            InboundObservationLogModel model)
        {
            AstroCueUser user = this._context.AstroCueUsers
                .Include(u => u.ObservationLogs)
                .Single(u => u.Id == reqUserId);

            // prevent duplicate logs being created
            if (user.ObservationLogs.Any(log => log.LogForReportId == model.ReportId))
            {
                throw new Exception("You have already created a log against this report!");
            }

            Report report = this._context.Reports
                .Include(r => r.ObservationLocation)
                .SingleOrDefault(r => r.Id == model.ReportId && r.AstroCueUserId == reqUserId);

            if (report == null)
            {
                throw new Exception("Report does not exist on account");
            }

            ObservationLog log = new()
            {
                TextualDescription = model.TextualDescription.Trim(),
                Observer = string.IsNullOrWhiteSpace(model.Observer) ? null : model.Observer,

                WeatherForecast = report.WeatherForecast,
                ObservationLocationName = report.ObservationLocation.Name,
                ObservationLocationLatitude = report.ObservationLocation.Latitude,
                ObservationLocationLongitude = report.ObservationLocation.Longitude,

                ObservedAstronomicalObjectName = report.AstronomicalObjectName,
                CalculatedBestTimeToObserveUtc = report.BestTimeToObserveUtc,
                HorizontalCoordinates = report.HorizontalCoordinates,

                TypeOfObservation = model.ObservationType switch
                {
                    "NakedEye" => "Naked eye",
                    "LongExposure" => "Long exposure photography",
                    _ => model.ObservationType
                },

                DateTaken = DateTime.Now.ToUniversalTime(),
                LogForReportId = model.ReportId
            };

            user.ObservationLogs.Add(log);

            return this._context.SaveChanges() == 1
                ? this._mapper.Map<OutboundObservationLogModel>(log)
                : null;
        }
    }
}