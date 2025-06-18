using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetAllSubcategoriesWithPaginationQuery
{
    public class GetAllSubcategoriesWithPaginationQueryHandler(IAppDbContext context, IMapper mapper, IUser userContext, IIncludeConfigurator<Subcategory> includeConfigurator) : IRequestHandler<GetAllSubcategoriesWithPaginationQuery, Result<PaginatedList<ReadSubcategoryDto>>>
    {
        public async Task<Result<PaginatedList<ReadSubcategoryDto>>> Handle(GetAllSubcategoriesWithPaginationQuery request, CancellationToken cancellationToken = default)
        {
            IncludeBuilder<Subcategory> builder = new(includeConfigurator, request.Include, userContext.Roles);
            IQueryable<Subcategory> query = builder.Apply(context.Subcategories.AsNoTracking());

            PaginatedList<ReadSubcategoryDto> dtos = await query
                .ProjectTo<ReadSubcategoryDto>(mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            return dtos;
        }
    }
}
