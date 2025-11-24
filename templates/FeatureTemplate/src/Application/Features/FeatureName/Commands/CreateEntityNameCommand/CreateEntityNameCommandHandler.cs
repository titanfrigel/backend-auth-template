using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Domain.Entities;
using MediatR;

namespace BackendAuthTemplate.Application.Features.FeatureName.Commands.CreateEntityNameCommand
{
    public class CreateEntityNameCommandHandler(IAppDbContext context) : IRequestHandler<CreateEntityNameCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateEntityNameCommand request, CancellationToken cancellationToken = default)
        {
            if (context.FeatureName.Any(c => c.Name == request.Name))
            {
                return FeatureNameErrors.Conflict("name");
            }

            EntityName entityName = new() { Name = request.Name };

            _ = context.FeatureName.Add(entityName);

            _ = await context.SaveChangesAsync(cancellationToken);

            return entityName.Id;
        }
    }
}
