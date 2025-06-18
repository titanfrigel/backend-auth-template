using AutoMapper;
using BackendAuthTemplate.Application.Features.Categories.Commands.CreateCategoryCommand;

namespace BackendAuthTemplate.API.Requests.Categories
{
    public class CreateCategoryRequest
    {
        public required string Name { get; init; }
        public required string Description { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<CreateCategoryRequest, CreateCategoryCommand>();
            }
        }
    }
}
