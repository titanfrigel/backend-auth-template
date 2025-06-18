using AutoMapper;
using BackendAuthTemplate.Application.Features.Categories.Commands.UpdateCategoryCommand;

namespace BackendAuthTemplate.API.Requests.Categories
{
    public class UpdateCategoryRequest
    {
        public required string Name { get; init; }
        public required string Description { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<UpdateCategoryRequest, UpdateCategoryCommand>()
                    .ForMember(dest => dest.CategoryId, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["CategoryId"]));
            }
        }
    }
}
