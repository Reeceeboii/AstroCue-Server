namespace AstroCue.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Astronomy;
    using AutoMapper;
    using Controllers.Parameters;
    using Data;
    using Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models.API.Outbound;

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
        /// <returns>A list of <see cref="OutboundAstronomialObjectModel"/> instances
        /// that match the search queries</returns>
        public IList<OutboundAstronomialObjectModel> ObjectSearch(AstronomicalObjectSearchParams searchParams, int reqUserId)
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

            // apply search term, using prefix wildcard for efficient indexing
            string searchTerm = $"{searchParams.Query.Trim()}%";
            query = query
                .Where(obj => EF.Functions.Like(obj.Name, searchTerm));

            // limit query and execute
            query = query.Take(searchParams.Limit);
            IList<AstronomicalObject> results = query.ToList();

            IList<OutboundAstronomialObjectModel> outbounds =
                this._mapper.Map<IList<OutboundAstronomialObjectModel>>(results);

            foreach (OutboundAstronomialObjectModel model in outbounds)
            {
                // Generate a CDS Portal search link so more information about the object can be found
                model.MoreInformation = searchParams.Type == "Star"
                    ? $"http://cdsportal.u-strasbg.fr/?target=hip+{model.CatalogueIdentifier}"
                    : $"http://cdsportal.u-strasbg.fr/?target=ngc+{model.CatalogueIdentifier}";

                // if the location was set from SingleOrDefault
                if (loc.Id <= 0) continue;

                if (model.ApparentMagnitude > BortleScale.ScaleToNakedEyeLimitingMagnitude(loc.BortleScaleValue))
                {
                    model.VisibilityAlert = true;
                    model.VisibilityMessage = $"This object is too dim to be seen with with the naked-eye from {loc.Name}, " +
                                              "however, you may still be able to see it with telescopes, binoculars, or long " +
                                              "exposure photography";
                }
                else
                {
                    model.VisibilityAlert = false;
                    model.VisibilityMessage = $"This object is bright enough to be seen from {loc.Name}";
                }
            }

            return outbounds;
        }
    }
}
