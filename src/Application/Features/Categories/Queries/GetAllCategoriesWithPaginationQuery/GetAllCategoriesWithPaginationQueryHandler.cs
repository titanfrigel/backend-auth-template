using AutoMapper;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetAllCategoriesWithPaginationQuery
{
    public class GetAllCategoriesWithPaginationQueryHandler(IAppDbContext context, IMapper mapper, IUser userContext, IIdentityService identityService, IIncludeConfigurator<Category> includeConfigurator) : IRequestHandler<GetAllCategoriesWithPaginationQuery, Result<PaginatedList<ReadCategoryDto>>>
    {
        public async Task<Result<PaginatedList<ReadCategoryDto>>> Handle(GetAllCategoriesWithPaginationQuery request, CancellationToken cancellationToken = default)
        {
            IncludeBuilder<Category> builder = new(includeConfigurator, request.Include, userContext.Roles);
            IQueryable<Category> query = builder.Apply(context.Categories.AsNoTracking());

            PaginatedList<Category> categories = await query
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            bool includeCreatedBy = builder.tree.ContainsKey("createdBy");

            Dictionary<Guid, ReadUserDto>? createdByUsers = includeCreatedBy
                ? (await identityService.GetUsersByIdsAsync(categories.Items.Select(x => x.CreatedById).ToList(), cancellationToken)).ToDictionary(x => x.Id, x => x)
                : null;

            PaginatedList<ReadCategoryDto> dtos = mapper.MapPaginated<ReadCategoryDto, Category>(categories, category => opts => opts.Items["CreatedBy"] = createdByUsers?[category.Id]);

            return dtos;
        }
    }
}
