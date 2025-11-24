using AutoMapper;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;

namespace BackendAuthTemplate.Application.Features.FeatureName.Dtos
{
    public class ReadEntityNameDto
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public ReadUserDto? CreatedBy { get; init; } = null;

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<EntityName, ReadEntityNameDto>()
                    .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["CreatedBy"]));
            }
        }
    }
}
