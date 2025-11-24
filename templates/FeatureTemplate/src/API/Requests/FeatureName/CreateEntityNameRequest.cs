using AutoMapper;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.CreateEntityNameCommand;

namespace BackendAuthTemplate.API.Requests.FeatureName
{
    public class CreateEntityNameRequest
    {
        public required string Name { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<CreateEntityNameRequest, CreateEntityNameCommand>();
            }
        }
    }
}
