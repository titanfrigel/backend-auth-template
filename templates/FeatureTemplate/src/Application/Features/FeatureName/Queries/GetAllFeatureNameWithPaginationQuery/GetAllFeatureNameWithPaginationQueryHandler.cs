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

            List<ReadEntityNameDto> dtos = [];

            foreach (EntityName entityName in featureName.Items)
            {
                ReadUserDto? createdBy = null;

                if (builder.tree.ContainsKey("createdBy"))
                {
                    createdBy = await identityService.GetUserByIdAsync(entityName.CreatedById, cancellationToken);
                }

                ReadEntityNameDto dto = mapper.Map<ReadEntityNameDto>(entityName, opt => opt.Items["CreatedBy"] = createdBy);

                dtos.Add(dto);
            }

            return new PaginatedList<ReadEntityNameDto>()
            {
                Items = dtos,
                PageNumber = featureName.PageNumber,
                TotalPages = featureName.TotalPages,
                TotalCount = featureName.TotalCount
            };
        }
    }
}
