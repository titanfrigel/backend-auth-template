using AutoMapper;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.FeatureName.Queries.GetEntityNameByIdQuery
{
    public class GetEntityNameByIdQueryHandler(IAppDbContext context, IMapper mapper, IUser userContext, IIdentityService identityService, IIncludeConfigurator<EntityName> includeConfigurator) : IRequestHandler<GetEntityNameByIdQuery, Result<ReadEntityNameDto>>
    {
        public async Task<Result<ReadEntityNameDto>> Handle(GetEntityNameByIdQuery request, CancellationToken cancellationToken = default)
        {
            IncludeBuilder<EntityName> builder = new(includeConfigurator, request.Include, userContext.Roles);
            IQueryable<EntityName> query = builder.Apply(context.FeatureName.AsNoTracking());

            EntityName? entityName = await query
                .Where(c => c.Id == request.EntityNameId)
                .FirstOrDefaultAsync(cancellationToken);

            if (entityName == null)
            {
                return FeatureNameErrors.NotFound(request.EntityNameId);
            }

            ReadUserDto? createdBy = null;
            if (builder.tree.ContainsKey("createdBy"))
            {
                createdBy = await identityService.GetUserByIdAsync(entityName.CreatedById, cancellationToken);
            }

            ReadEntityNameDto dto = mapper.Map<ReadEntityNameDto>(entityName, opt => opt.Items["CreatedBy"] = createdBy);

            return dto;
        }
    }
}
