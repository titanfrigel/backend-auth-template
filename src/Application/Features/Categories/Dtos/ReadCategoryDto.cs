using AutoMapper;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Domain.Entities;

namespace BackendAuthTemplate.Application.Features.Categories.Dtos
{
    public class ReadCategoryDto
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public List<ReadSubcategoryDto>? Subcategories { get; init; } = null;

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<Category, ReadCategoryDto>();
            }
        }
    }
}
