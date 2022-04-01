namespace AstroCue.Server.Data
{
    using AutoMapper;
    using Entities;
    using Models.API.Inbound;
    using Models.API.Outbound;
    using Models.Email;

    /// <summary>
    /// Class to hold AutoMapper type mappings
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AutoMapperProfile"/> class
        /// </summary>
        public AutoMapperProfile()
        {
            // users and registration etc...
            this.CreateMap<InboundRegModel, AstroCueUser>();
            this.CreateMap<AstroCueUser, OutboundAuthSuccessModel>();

            // observation locations
            this.CreateMap<InboundObsLocationModel, ObservationLocation>();
            this.CreateMap<ObservationLocation, OutboundObsLocationModel>();

            // observations
            this.CreateMap<HipObject, OutboundAstronomialObjectModel>();
            this.CreateMap<NgcObject, OutboundAstronomialObjectModel>();
            this.CreateMap<Observation, OutboundObservationModel>();

            // email models
            this.CreateMap<AstroCueUser, WelcomeEmailModel>();

            // reports
            this.CreateMap<Report, OutboundReportModel>();
        }
    }
}
