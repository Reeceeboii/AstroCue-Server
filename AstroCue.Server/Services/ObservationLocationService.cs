namespace AstroCue.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data;
    using Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models.API.Inbound;
    using Models.API.Outbound;
    using Models.Misc;
    using Utilities;

    /// <summary>
    /// Service class for managing observation locations
    /// </summary>
    public class ObservationLocationService : IObservationLocationService
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
        /// Instance of <see cref="IWeatherForecastService"/>
        /// </summary>
        private readonly IWeatherForecastService _weatherForecastService;

        /// <summary>
        /// Instance of <see cref="IMappingService"/>
        /// </summary>
        private readonly IMappingService _mappingService;

        /// <summary>
        /// Instance of <see cref="ILightPollutionService"/>
        /// </summary>
        private readonly ILightPollutionService _lightPollutionService;

        /// <summary>
        /// Initialises a new instance of the <see cref="ObservationLocationService"/> class
        /// </summary>
        /// <param name="context">Instance of <see cref="ApplicationDbContext"/></param>
        /// <param name="mapper">Instance of <see cref="IMapper"/></param>
        /// <param name="weatherForecastService">Instance of <see cref="IWeatherForecastService"/></param>
        /// <param name="mappingService">Instance of <see cref="IMappingService"/></param>
        /// <param name="lightPollutionService">Instance of <see cref="ILightPollutionService"/></param>
        public ObservationLocationService(
            ApplicationDbContext context,
            IMapper mapper,
            IWeatherForecastService weatherForecastService,
            IMappingService mappingService,
            ILightPollutionService lightPollutionService)
        {
            this._context = context;
            this._mapper = mapper;
            this._weatherForecastService = weatherForecastService;
            this._mappingService = mappingService;
            this._lightPollutionService = lightPollutionService;
        }

        /// <summary>
        /// Add a new observation location to a user's account
        /// </summary>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <param name="inboundModel">An instance of <see cref="InboundObsLocationModel"/></param>
        /// <returns>An instance of <see cref="OutboundObsLocationModel"/></returns>
        public OutboundObsLocationModel AddNew(int reqUserId, InboundObsLocationModel inboundModel)
        {
            AstroCueUser user = this._context.AstroCueUsers
                .Where(u => u.Id == reqUserId)
                .Include(u => u.ObservationLocations)
                .Single();

            ObservationLocation loc = this._mapper.Map<ObservationLocation>(inboundModel);

            LightPollution pollution;

            try
            {
                pollution =
                    this._lightPollutionService.GetLightPollutionForCoords(loc.Longitude, loc.Latitude);
            }
            catch (Exception)
            {
                return null;
            }

            loc.Name = StringUtilities.TrimToUpperFirstChar(loc.Name);
            loc.BortleScaleValue = pollution.BortleValue;
            loc.BortleDesc = pollution.BortleDesc;

            user.ObservationLocations.Add(loc);

            return this._context.SaveChanges() == 1
                ? this._mapper.Map<OutboundObsLocationModel>(loc)
                : null;
        }

        /// <summary>
        /// Delete an observation location from a user's account
        /// </summary>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <param name="locationId">The ID of the <see cref="ObservationLocation"/> being deleted</param>
        /// <returns>An instance of <see cref="OutboundObsLocationModel"/>, or null</returns>
        public OutboundObsLocationModel Delete(int reqUserId, int locationId)
        {
            ObservationLocation loc = this._context.ObservationLocations
                .Include(o => o.Reports)
                .SingleOrDefault(l => l.AstroCueUserId == reqUserId && l.Id == locationId);

            if (loc == null)
            {
                throw new ArgumentException("Location does not exist on account");
            }

            if (loc.Reports.Any())
            {
                throw new ArgumentException(
                    "You cannot delete an observation location with associated reports");
            }

            this._context.ObservationLocations.Attach(loc);
            this._context.ObservationLocations.Remove(loc);

            return this._context.SaveChanges() == 1 
                ? this._mapper.Map<OutboundObsLocationModel>(loc) 
                : null;
        }

        /// <summary>
        /// Retrieve all of the observation locations for a given user
        /// </summary>
        /// <param name="reqUserId">The ID of the user</param>
        /// <returns>A list of <see cref="OutboundObsLocationModel"/> instances</returns>
        public async Task<IList<OutboundObsLocationModel>> GetAllAsync(int reqUserId)
        {
            List<ObservationLocation> observationLocations = this._context.ObservationLocations
                .Where(l => l.AstroCueUserId == reqUserId)
                .ToList();

            List<OutboundObsLocationModel> outboundObservationLocations =
                this._mapper.Map<List<OutboundObsLocationModel>>(observationLocations);

            // asynchronously gather a set of weather forecasts for each observation location in the list
            IEnumerable<Task> forecastTasks = outboundObservationLocations.Select(async location =>
            {
                location.SingleForecast =
                    await this._weatherForecastService.GetCurrentWeatherAsync(location.Longitude, location.Latitude);
            });

            await Task.WhenAll(forecastTasks);

            return outboundObservationLocations;
        }

        /// <summary>
        /// Gets a static map for a specific <see cref="ObservationLocation"/>
        /// </summary>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <param name="locationId">The ID of the <see cref="ObservationLocation"/> that the map is being retrieved for</param>
        /// <returns>A byte array representing the image data</returns>
        /// <exception cref="ArgumentException">If the user does not an <see cref="ObservationLocation"/> matching
        /// the given ID</exception>
        public async Task<byte[]> GetStaticMapAsync(int reqUserId, int locationId)
        {
            ObservationLocation location = this._context.ObservationLocations
                .SingleOrDefault(l => l.Id == locationId && l.AstroCueUserId == reqUserId);

            if (location == null)
            {
                throw new ArgumentException("Location does not exist on account");
            }

            return await this._mappingService.GetStaticMapImageAsync(location.Longitude, location.Latitude);
        }

        /// <summary>
        /// Gets a static map for a specific <see cref="ObservationLocation"/> and returns the resulting
        /// image as a base64 data URL
        /// </summary>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <param name="locationId">The ID of the <see cref="ObservationLocation"/> that the map is being retrieved for</param>
        /// <returns>A byte array representing the image data</returns>
        /// <exception cref="ArgumentException">If the user does not an <see cref="ObservationLocation"/> matching
        /// the given ID</exception>
        public async Task<string> GetStaticMapAsBase64Async(int reqUserId, int locationId)
        {
            ObservationLocation location = this._context.ObservationLocations
                .SingleOrDefault(l => l.Id == locationId && l.AstroCueUserId == reqUserId);

            if (location == null)
            {
                throw new ArgumentException("Location does not exist on account");
            }

            byte[] mapBytes = await this._mappingService.GetStaticMapImageAsync(location.Longitude, location.Latitude);
            return $"data:image/png;base64,{Convert.ToBase64String(mapBytes)}";
        }
    }
}
