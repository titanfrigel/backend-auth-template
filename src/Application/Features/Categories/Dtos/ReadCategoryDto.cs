using AutoMapper;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;

namespace BackendAuthTemplate.Application.Features.Categories.Dtos
{
    public class ReadCategoryDto
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public List<ReadSubcategoryDto>? Subcategories { get; init; } = null;
        public ReadUserDto? CreatedBy { get; init; } = null;

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<Category, ReadCategoryDto>()
                    .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["CreatedBy"]));
            }
        }
    }
}
