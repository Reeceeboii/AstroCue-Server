﻿namespace AstroCue.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Astronomy;
    using AutoMapper;
    using Controllers.Parameters;
    using Data;
    using Entities;
    using Entities.Owned;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models.API.Outbound;
    using Models.Misc;

    /// <summary>
    /// Service class for handling astronomical observation operations
    /// </summary>
    public class ObservationService : IObservationService
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
        /// Initialises an instance of the <see cref="ObservationService"/> class
        /// </summary>
        /// <param name="context">Instance of <see cref="ApplicationDbContext"/></param>
        /// <param name="mapper">Instance of <see cref="IMapper"/></param>
        public ObservationService(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        /// <summary>
        /// Search the database for astronomical objects with a set of parameters
        /// </summary>
        /// <param name="searchParams">An instance of <see cref="AstronomicalObjectSearchParams"/></param>
        /// <param name="reqUserId">ID of the user that made the request</param>
        /// <returns>A list of <see cref="OutboundAstronomicalObjectModel"/> instances
        /// that match the search queries</returns>
        public IList<OutboundAstronomicalObjectModel> ObjectSearch(AstronomicalObjectSearchParams searchParams, int reqUserId)
        {
            ObservationLocation loc = new()
            {
                Id = 0
            };

            // if the user has provided the ID of one of their observation locations to the query
            if (searchParams.LocationId > 0)
            {
                loc = this._context.ObservationLocations
                    .SingleOrDefault(l => l.AstroCueUserId == reqUserId && l.Id == searchParams.LocationId);
                
                if (loc == null)
                {
                    throw new ArgumentException("No location with given ID on user account",
                        nameof(searchParams));
                }
            }

            // start a query
            IQueryable<AstronomicalObject> query = searchParams.Type == "Star"
                ? this._context.HipObjects
                : this._context.NgcObjects;

            // apply popular filter if applicable
            if (searchParams.Popular)
            {
                query = query.Where(obj => obj.OfficiallyNamed);
            }

            // using a non prefix wildcard isn't as efficient but so be it
            string searchTerm = $"%{searchParams.Query.Trim()}%";
            query = query
                .Where(obj => EF.Functions.Like(obj.Name, searchTerm));

            // limit query and execute
            query = query
                .Take(searchParams.Limit)
                .OrderBy(r => r.Name);

            IList<AstronomicalObject> results = query.ToList();

            IList<OutboundAstronomicalObjectModel> outbounds =
                this._mapper.Map<IList<OutboundAstronomicalObjectModel>>(results);

            foreach (OutboundAstronomicalObjectModel model in outbounds)
            {
                // Generate a CDS Portal search link so more information about the object can be found
                model.MoreInformation = GenerateMoreInformationUrl(searchParams.Type, model.CatalogueIdentifier);

                // if the location was not set from SingleOrDefault
                if (loc.Id <= 0) continue;

                // apply short visibility report
                model.LocationVisibilityModel = new LocationVisibilityModel();

                /*
                 * Track diurnal motion over the next 24 hrs to ascertain whether or not the object actually rises
                 * above the observer's local horizon.
                 * See https://www.astronomy.ohio-state.edu/pogge.1/Ast161/Unit2/Images/circumpolar_sm.gif for
                 * a simple visualisation.
                 *
                 * We essentially want to apply a warning to any object whose altitude is never >0deg over the
                 * next 24hrs' worth of motion
                 */

                // only ever going be n<10 objects so O(n) not really a concern here
                AstronomicalObject tempObject = results.Single(r => r.Id == model.Id);
                List<AltAz> diurnalMotion = new();

                DateTime start = DateTime.Now.ToUniversalTime().AddHours(1);
                for (DateTime instant = start; instant <= start.AddDays(1); instant = instant.AddHours(1))
                {
                    diurnalMotion.Add(CoordinateTransformations.EquatorialToHorizontal(
                        tempObject,
                        instant, 
                        loc.Longitude,
                        loc.Latitude));
                }

                bool rises = diurnalMotion.Any(coord => coord.Altitude > 0);

                // and finally, apply the warnings to the result set
                if (!rises)
                {
                    model.LocationVisibilityModel.HorizonAlert = true;
                    model.LocationVisibilityModel.HorizonMessage = $"Object not visible over next 24hrs from {loc.Name}";
                }

                if (model.ApparentMagnitude > BortleScale.ScaleToNakedEyeLimitingMagnitude(loc.BortleScaleValue))
                {
                    model.LocationVisibilityModel.VisibilityAlert = true;
                    model.LocationVisibilityModel.VisibilityMessage =
                        $"Too dim for naked-eye observations from {loc.Name}, but " +
                        "you may still observe it with telescopes or long exposure photography";
                }
                else
                {
                    model.LocationVisibilityModel.VisibilityAlert = false;
                    model.LocationVisibilityModel.VisibilityMessage =
                        $"This object is bright enough to be seen from {loc.Name}!";
                }
            }

            return outbounds;
        }

        /// <summary>
        /// Set up a new observation between a location and an astronomical object
        /// </summary>
        /// <param name="locationId">The ID of the location where the observation will take place</param>
        /// <param name="astronomicalObjectId">The ID of the astronomical object that is being observed</param>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <returns></returns>
        public OutboundObservationModel NewObservation(int locationId, int astronomicalObjectId, int reqUserId)
        {
            ObservationLocation loc = this._context.ObservationLocations
                .Include(l => l.Observations)
                .SingleOrDefault(l => l.AstroCueUserId == reqUserId && l.Id == locationId);

            AstronomicalObject obj = this._context.AstronomicalObjects
                .SingleOrDefault(obj => obj.Id == astronomicalObjectId);

            if (loc == null)
            {
                throw new ArgumentException("No location with given ID on user account", nameof(reqUserId));
            }

            if (obj == null)
            {
                throw new ArgumentException("No astronomical object with ID", nameof(astronomicalObjectId));
            }

            // check for duplicates
            if (loc.Observations.Any(o => o.AstronomicalObjectId == astronomicalObjectId))
            {
                throw new ArgumentException($"You already have an observation between {obj.Name} and {loc.Name}!");
            }

            Observation newObs = new()
            {
                ObservationLocation = loc,
                ObservationLocationId = loc.Id,
                AstronomicalObject = obj,
                AstronomicalObjectId = astronomicalObjectId
            };

            loc.Observations.Add(newObs);

            return this._context.SaveChanges() == 1 
                ? this._mapper.Map<OutboundObservationModel>(newObs) 
                : null;
        }

        /// <summary>
        /// Retrieve all of the observations belonging to a certain user
        /// </summary>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <returns>A list of <see cref="OutboundObservationModel"/> instances</returns>
        public IList<OutboundObservationModel> GetAll(int reqUserId)
        {
            IList<Observation> models = this._context.Observations
                .Include(o => o.ObservationLocation)
                .Include(o => o.AstronomicalObject)
                .Where(o => o.ObservationLocation.AstroCueUserId == reqUserId)
                .ToList();

            // generate more information URLs for each observation
            IList<OutboundObservationModel> outbound = this._mapper.Map<IList<OutboundObservationModel>>(models);
            foreach (OutboundObservationModel obs in outbound)
            {
                obs.AstronomicalObject.MoreInformation = GenerateMoreInformationUrl(
                    obs.AstronomicalObject.Type, obs.AstronomicalObject.CatalogueIdentifier);
            }

            return outbound;
        }

        /// <summary>
        /// Delete an existing observation from an account
        /// </summary>
        /// <param name="observationId">The ID of the observation to delete</param>
        /// <param name="reqUserId">The ID of the user that made the request</param>
        /// <returns>An instance of <see cref="OutboundObservationModel"/></returns>
        public OutboundObservationModel DeleteObservation(int observationId, int reqUserId)
        {
            Observation obs = this._context.Observations
                .Include(o => o.ObservationLocation)
                .SingleOrDefault(o => o.Id == observationId && o.ObservationLocation.AstroCueUserId == reqUserId);

            if (obs == null)
            {
                throw new ArgumentException("Observation does not exist on this account", nameof(observationId));
            }

            this._context.Observations.Attach(obs);
            this._context.Remove(obs);

            return this._context.SaveChanges() == 1
                ? this._mapper.Map<OutboundObservationModel>(obs)
                : null;
        }

        /// <summary>
        /// Generates a CDS Portal search link so more information about an object can be found
        /// </summary>
        /// <param name="objectType">The type of object, "Star", or "DeepSky"</param>
        /// <param name="catalogueIdentifier">A catalogue identifier for the object</param>
        /// <returns>A link to the CDS portal for the object provided</returns>
        public static string GenerateMoreInformationUrl(string objectType, int catalogueIdentifier)
        {
            return objectType == "Star"
                ? $"http://cdsportal.u-strasbg.fr/?target=hip+{catalogueIdentifier}"
                : $"http://cdsportal.u-strasbg.fr/?target=ngc+{catalogueIdentifier}";
        }
    }
}
