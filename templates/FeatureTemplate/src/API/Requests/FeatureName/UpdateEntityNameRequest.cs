using AutoMapper;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.UpdateEntityNameCommand;

namespace BackendAuthTemplate.API.Requests.FeatureName
{
    public class UpdateEntityNameRequest
    {
        public required string Name { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<UpdateEntityNameRequest, UpdateEntityNameCommand>()
                    .ForMember(dest => dest.EntityNameId, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["EntityNameId"]));
            }
        }
    }
}
