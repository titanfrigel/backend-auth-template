using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Domain.Entities;
using MediatR;

namespace BackendAuthTemplate.Application.Features.FeatureName.Commands.CommandNameCommand
{
    public class CommandNameCommandHandler(IAppDbContext context) : IRequestHandler<CommandNameCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CommandNameCommand request, CancellationToken cancellationToken = default)
        {
            EntityName entityName = new()
            {
                Name = request.Name
            };

            _ = context.FeatureName.Add(entityName);

            _ = await context.SaveChangesAsync(cancellationToken);

            return entityName.Id;
        }
    }
}
