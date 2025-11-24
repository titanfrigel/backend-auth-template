using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.FeatureName.Commands.UpdateEntityNameCommand
{
    public class UpdateEntityNameCommand : IRequest<Result>
    {
        public required Guid EntityNameId { get; init; }
        public required string Name { get; init; }
    }
}
