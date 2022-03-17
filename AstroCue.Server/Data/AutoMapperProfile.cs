namespace AstroCue.Server.Data
{
    using AutoMapper;
    using Entities;
    using Models.API.Inbound;
    using Models.API.Outbound;

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
            //CreateMap<InboundObservationLocationModel, ObservationLocation>();
            //CreateMap<ObservationLocation, OutboundObsLocationModel>();

            // email models
            //CreateMap<AstroCueUser, WelcomeEmailModel>();
        }
    }
}
