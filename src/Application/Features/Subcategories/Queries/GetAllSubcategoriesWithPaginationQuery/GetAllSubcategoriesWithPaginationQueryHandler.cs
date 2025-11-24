using AutoMapper;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetAllSubcategoriesWithPaginationQuery
{
    public class GetAllSubcategoriesWithPaginationQueryHandler(IAppDbContext context, IMapper mapper, IUser userContext, IIdentityService identityService, IIncludeConfigurator<Subcategory> includeConfigurator) : IRequestHandler<GetAllSubcategoriesWithPaginationQuery, Result<PaginatedList<ReadSubcategoryDto>>>
    {
        public async Task<Result<PaginatedList<ReadSubcategoryDto>>> Handle(GetAllSubcategoriesWithPaginationQuery request, CancellationToken cancellationToken = default)
        {
            IncludeBuilder<Subcategory> builder = new(includeConfigurator, request.Include, userContext.Roles);
            IQueryable<Subcategory> query = builder.Apply(context.Subcategories.AsNoTracking());

            PaginatedList<Subcategory> subcategories = await query
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            bool includeCreatedBy = builder.tree.ContainsKey("createdBy");

            Dictionary<Guid, ReadUserDto>? createdByUsers = includeCreatedBy
                ? (await identityService.GetUsersByIdsAsync(subcategories.Items.Select(x => x.CreatedById).ToList(), cancellationToken)).ToDictionary(x => x.Id, x => x)
                : null;

            PaginatedList<ReadSubcategoryDto> dtos = mapper.MapPaginated<ReadSubcategoryDto, Subcategory>(subcategories, subcategory => opts => opts.Items["CreatedBy"] = createdByUsers?[subcategory.Id]);

            return dtos;
        }
    }
}
