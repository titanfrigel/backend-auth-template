using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.FeatureName.Commands.CommandNameCommand
{
    public class CommandNameCommand : IRequest<Result<Guid>>
    {
        public required string Name { get; init; }
    }
}
