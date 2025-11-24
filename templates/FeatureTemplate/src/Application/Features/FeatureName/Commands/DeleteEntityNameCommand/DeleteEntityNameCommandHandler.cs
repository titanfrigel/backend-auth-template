using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.FeatureName.Commands.DeleteEntityNameCommand
{
    public class DeleteEntityNameCommandHandler(IAppDbContext context) : IRequestHandler<DeleteEntityNameCommand, Result>
    {
        public async Task<Result> Handle(DeleteEntityNameCommand request, CancellationToken cancellationToken = default)
        {
            EntityName? entityName = await context.FeatureName
                .FirstOrDefaultAsync(c => c.Id == request.EntityNameId, cancellationToken);

            if (entityName == null)
            {
                return FeatureNameErrors.NotFound(request.EntityNameId);
            }

            _ = context.FeatureName.Remove(entityName);

            _ = await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
