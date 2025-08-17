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

            List<ReadCategoryDto> dtos = [];

            foreach (Category category in categories.Items)
            {
                ReadUserDto? createdBy = null;

                if (builder.tree.ContainsKey("createdBy"))
                {
                    createdBy = await identityService.GetUserByIdAsync(category.CreatedById, cancellationToken);
                }

                ReadCategoryDto dto = mapper.Map<ReadCategoryDto>(category, opt => opt.Items["CreatedBy"] = createdBy);

                dtos.Add(dto);
            }

            return new PaginatedList<ReadCategoryDto>()
            {
                Items = dtos,
                PageNumber = categories.PageNumber,
                TotalPages = categories.TotalPages,
                TotalCount = categories.TotalCount
            };
        }
    }
}
