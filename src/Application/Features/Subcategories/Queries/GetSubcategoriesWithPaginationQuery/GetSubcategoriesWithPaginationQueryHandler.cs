using AutoMapper;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoriesWithPaginationQuery
{
    public class GetSubcategoriesWithPaginationQueryHandler(
        IAppDbContext context,
        IMapper mapper,
        IUser userContext,
        IIdentityService identityService,
        IIncludeConfigurator<Subcategory> includeConfigurator,
        ISortConfigurator<Subcategory> sortConfigurator) : IRequestHandler<GetSubcategoriesWithPaginationQuery, Result<PaginatedList<ReadSubcategoryDto>>>
    {
        public async Task<Result<PaginatedList<ReadSubcategoryDto>>> Handle(GetSubcategoriesWithPaginationQuery request, CancellationToken cancellationToken = default)
        {
            IQueryable<Subcategory> query = context.Subcategories.AsNoTracking()
                .ApplyIncludes(request, includeConfigurator, userContext, out HashSet<string>? validatedIncludes);

            query = query.ApplySorting(request, sortConfigurator);

            PaginatedList<Subcategory> paginatedSubcategories = await query
                .ToPaginatedListAsync(request, cancellationToken);

            Dictionary<Guid, ReadUserDto>? createdByUsers = null;
            if (validatedIncludes.Contains("CreatedBy"))
            {
                List<Guid> createdByIds = paginatedSubcategories.Items.Select(c => c.CreatedById).ToList();
                createdByUsers = (await identityService.GetUsersByIdsAsync(createdByIds, cancellationToken))
                    .ToDictionary(u => u.Id, u => u);
            }

            PaginatedList<ReadSubcategoryDto> dtos = mapper.MapPaginated<ReadSubcategoryDto, Subcategory>(paginatedSubcategories, subcategory => opts =>
            {
                opts.Items["CreatedBy"] = createdByUsers?[subcategory.CreatedById] ?? null;
            });

            return dtos;
        }
    }
}
