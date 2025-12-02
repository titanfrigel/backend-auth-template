using AutoMapper;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoriesWithPaginationQuery
{
    public class GetCategoriesWithPaginationQueryHandler(
        IAppDbContext context,
        IMapper mapper,
        IUser userContext,
        IIdentityService identityService,
        IIncludeConfigurator<Category> includeConfigurator,
        ISortConfigurator<Category> sortConfigurator) : IRequestHandler<GetCategoriesWithPaginationQuery, Result<PaginatedList<ReadCategoryDto>>>
    {
        public async Task<Result<PaginatedList<ReadCategoryDto>>> Handle(GetCategoriesWithPaginationQuery request, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> query = context.Categories.AsNoTracking()
                .ApplyIncludes(request, includeConfigurator, userContext, out HashSet<string>? validatedIncludes);

            query = query.ApplySorting(request, sortConfigurator);

            PaginatedList<Category> paginatedCategories = await query
                .ToPaginatedListAsync(request, cancellationToken);

            Dictionary<Guid, ReadUserDto>? createdByUsers = null;
            if (validatedIncludes.Contains("CreatedBy"))
            {
                List<Guid> createdByIds = paginatedCategories.Items.Select(c => c.CreatedById).ToList();
                createdByUsers = (await identityService.GetUsersByIdsAsync(createdByIds, cancellationToken))
                    .ToDictionary(u => u.Id, u => u);
            }

            PaginatedList<ReadCategoryDto> dtos = mapper.MapPaginated<ReadCategoryDto, Category>(paginatedCategories, category => opts =>
            {
                opts.Items["CreatedBy"] = createdByUsers?[category.CreatedById] ?? null;
            });

            return dtos;
        }
    }
}
