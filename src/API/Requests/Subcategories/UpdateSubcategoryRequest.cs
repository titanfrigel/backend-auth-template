using AutoMapper;
using BackendAuthTemplate.Application.Features.Subcategories.Commands.UpdateSubcategoryCommand;

namespace BackendAuthTemplate.API.Requests.Subcategories
{
    public class UpdateSubcategoryRequest
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required Guid CategoryId { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<UpdateSubcategoryRequest, UpdateSubcategoryCommand>()
                    .ForMember(dest => dest.SubcategoryId, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["SubcategoryId"]));
            }
        }
    }
}