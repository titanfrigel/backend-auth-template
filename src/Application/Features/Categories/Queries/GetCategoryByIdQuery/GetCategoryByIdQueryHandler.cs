using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Application.Features.Subcategories;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoryByIdQuery
{
    public class GetCategoryByIdQueryHandler(IAppDbContext context, IMapper mapper, IUser userContext, IIdentityService identityService, IIncludeConfigurator<Category> includeConfigurator) : IRequestHandler<GetCategoryByIdQuery, Result<ReadCategoryDto>>
    {
        public async Task<Result<ReadCategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken = default)
        {
            IncludeBuilder<Category> builder = new(includeConfigurator, request.Include, userContext.Roles);
            IQueryable<Category> query = builder.Apply(context.Categories.AsNoTracking());

            Category? category = await query
                .Where(c => c.Id == request.CategoryId)
                .FirstOrDefaultAsync(cancellationToken);

            if (category == null)
            {
                return CategoriesErrors.NotFound(request.CategoryId);
            }

            ReadUserDto? createdBy = null;
            if (builder.tree.ContainsKey("createdBy"))
            {
                createdBy = await identityService.GetUserByIdAsync(category.CreatedById, cancellationToken);
            }

            ReadCategoryDto dto = mapper.Map<ReadCategoryDto>(category, opt => opt.Items["CreatedBy"] = createdBy);

            return dto;
        }
    }
}
