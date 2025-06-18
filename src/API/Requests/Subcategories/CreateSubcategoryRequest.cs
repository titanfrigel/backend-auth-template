using AutoMapper;
using BackendAuthTemplate.Application.Features.Subcategories.Commands.CreateSubcategoryCommand;

namespace BackendAuthTemplate.API.Requests.Subcategories
{
    public class CreateSubcategoryRequest
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required Guid CategoryId { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<CreateSubcategoryRequest, CreateSubcategoryCommand>();
            }
        }
    }
}