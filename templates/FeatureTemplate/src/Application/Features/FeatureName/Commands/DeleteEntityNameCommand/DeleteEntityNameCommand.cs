using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.FeatureName.Commands.DeleteEntityNameCommand
{
    public class DeleteEntityNameCommand : IRequest<Result>
    {
        public required Guid EntityNameId { get; init; }
    }
}
