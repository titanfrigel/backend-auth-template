using AutoMapper;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
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

            List<ReadSubcategoryDto> dtos = [];

            foreach (Subcategory subcategory in subcategories.Items)
            {
                ReadUserDto? createdBy = null;

                if (builder.tree.ContainsKey("createdBy"))
                {
                    createdBy = await identityService.GetUserByIdAsync(subcategory.CreatedById, cancellationToken);
                }

                ReadSubcategoryDto dto = mapper.Map<ReadSubcategoryDto>(subcategory, opt => opt.Items["CreatedBy"] = createdBy);

                dtos.Add(dto);
            }

            return new PaginatedList<ReadSubcategoryDto>()
            {
                Items = dtos,
                PageNumber = subcategories.PageNumber,
                TotalPages = subcategories.TotalPages,
                TotalCount = subcategories.TotalCount
            };
        }
    }
}
