using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetAllCategoriesWithPaginationQuery
{
    public class GetAllCategoriesWithPaginationQueryHandler(IAppDbContext context, IMapper mapper, IUser userContext, IIncludeConfigurator<Category> includeConfigurator) : IRequestHandler<GetAllCategoriesWithPaginationQuery, Result<PaginatedList<ReadCategoryDto>>>
    {
        public async Task<Result<PaginatedList<ReadCategoryDto>>> Handle(GetAllCategoriesWithPaginationQuery request, CancellationToken cancellationToken = default)
        {
            IncludeBuilder<Category> builder = new(includeConfigurator, request.Include, userContext.Roles);
            IQueryable<Category> query = builder.Apply(context.Categories.AsNoTracking());

            PaginatedList<ReadCategoryDto> dtos = await query
                .ProjectTo<ReadCategoryDto>(mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            return dtos;
        }
    }
}
