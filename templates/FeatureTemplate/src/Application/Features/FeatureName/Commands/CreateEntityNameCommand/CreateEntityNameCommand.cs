using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.FeatureName.Commands.CreateEntityNameCommand
{
    public class CreateEntityNameCommand : IRequest<Result<Guid>>
    {
        public required string Name { get; init; }
    }
}
