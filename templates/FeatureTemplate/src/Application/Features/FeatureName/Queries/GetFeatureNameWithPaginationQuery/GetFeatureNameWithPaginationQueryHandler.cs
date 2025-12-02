using AutoMapper;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.FeatureName.Queries.GetFeatureNameWithPaginationQuery
{
    public class GetFeatureNameWithPaginationQueryHandler(
        IAppDbContext context,
        IMapper mapper,
        IUser userContext,
        IIdentityService identityService,
        IIncludeConfigurator<EntityName> includeConfigurator,
        ISortConfigurator<EntityName> sortConfigurator) : IRequestHandler<GetFeatureNameWithPaginationQuery, Result<PaginatedList<ReadEntityNameDto>>>
    {
        public async Task<Result<PaginatedList<ReadEntityNameDto>>> Handle(GetFeatureNameWithPaginationQuery request, CancellationToken cancellationToken = default)
        {
            IQueryable<EntityName> query = context.FeatureName.AsNoTracking()
                .ApplyIncludes(request, includeConfigurator, userContext, out var validatedIncludes);

            query = query.ApplySorting(request, sortConfigurator);

            PaginatedList<EntityName> paginatedFeatureName = await query
                .ToPaginatedListAsync(request, cancellationToken);

            Dictionary<Guid, ReadUserDto>? createdByUsers = null;
            if (validatedIncludes.Contains("CreatedBy"))
            {
                List<Guid> createdByIds = paginatedFeatureName.Items.Select(c => c.CreatedById).ToList();
                createdByUsers = (await identityService.GetUsersByIdsAsync(createdByIds, cancellationToken))
                    .ToDictionary(u => u.Id, u => u);
            }

            PaginatedList<ReadEntityNameDto> dtos = mapper.MapPaginated<ReadEntityNameDto, EntityName>(paginatedFeatureName, entityName => opts =>
            {
                opts.Items["CreatedBy"] = createdByUsers?[entityName.CreatedById] ?? null;
            });

            return dtos;
        }
    }
}
