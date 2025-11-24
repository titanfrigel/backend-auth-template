using AutoMapper;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.FeatureName.Queries.GetAllFeatureNameWithPaginationQuery
{
    public class GetAllFeatureNameWithPaginationQueryHandler(IAppDbContext context, IMapper mapper, IUser userContext, IIdentityService identityService, IIncludeConfigurator<EntityName> includeConfigurator) : IRequestHandler<GetAllFeatureNameWithPaginationQuery, Result<PaginatedList<ReadEntityNameDto>>>
    {
        public async Task<Result<PaginatedList<ReadEntityNameDto>>> Handle(GetAllFeatureNameWithPaginationQuery request, CancellationToken cancellationToken = default)
        {
            IncludeBuilder<EntityName> builder = new(includeConfigurator, request.Include, userContext.Roles);
            IQueryable<EntityName> query = builder.Apply(context.FeatureName.AsNoTracking());

            PaginatedList<EntityName> featureName = await query
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            bool includeCreatedBy = builder.tree.ContainsKey("createdBy");

            Dictionary<Guid, ReadUserDto>? createdByUsers = includeCreatedBy
                ? (await identityService.GetUsersByIdsAsync(featureName.Items.Select(x => x.CreatedById).ToList(), cancellationToken)).ToDictionary(x => x.Id, x => x)
                : null;

            PaginatedList<ReadEntityNameDto> dtos = mapper.MapPaginated<ReadEntityNameDto, EntityName>(featureName, entityName => opts => opts.Items["CreatedBy"] = createdByUsers?[entityName.CreatedById]);

            return dtos;
        }
    }
}
