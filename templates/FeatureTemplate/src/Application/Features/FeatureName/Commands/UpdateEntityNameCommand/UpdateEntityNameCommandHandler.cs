using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.FeatureName.Commands.UpdateEntityNameCommand
{
    public class UpdateEntityNameCommandHandler(IAppDbContext context) : IRequestHandler<UpdateEntityNameCommand, Result>
    {
        public async Task<Result> Handle(UpdateEntityNameCommand request, CancellationToken cancellationToken = default)
        {
            EntityName? entityName = await context.FeatureName.FirstOrDefaultAsync(c => c.Id == request.EntityNameId, cancellationToken);

            if (entityName == null)
            {
                return FeatureNameErrors.NotFound(request.EntityNameId);
            }

            if (context.FeatureName.Any(c => c.Name == request.Name && c.Id != request.EntityNameId))
            {
                return FeatureNameErrors.Conflict("name");
            }

            entityName.Name = request.Name;

            _ = context.FeatureName.Update(entityName);

            _ = await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
