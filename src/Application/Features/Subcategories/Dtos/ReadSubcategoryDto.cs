using AutoMapper;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;

namespace BackendAuthTemplate.Application.Features.Subcategories.Dtos
{
    public class ReadSubcategoryDto
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required Guid CategoryId { get; init; }
        public ReadCategoryDto? Category { get; init; } = null;
        public ReadUserDto? CreatedBy { get; init; } = null;

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<Subcategory, ReadSubcategoryDto>()
                    .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["CreatedBy"]));
            }
        }
    }
}
