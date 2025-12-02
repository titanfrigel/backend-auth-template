using AutoMapper;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
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
            IQueryable<Category> query = context.Categories.AsNoTracking()
                .ApplyIncludes(request, includeConfigurator, userContext, out HashSet<string>? validatedIncludes);

            Category? category = await query
                .Where(c => c.Id == request.CategoryId)
                .FirstOrDefaultAsync(cancellationToken);

            if (category == null)
            {
                return CategoriesErrors.NotFound(request.CategoryId);
            }

            ReadUserDto? createdBy = null;
            if (validatedIncludes.Contains("CreatedBy"))
            {
                createdBy = await identityService.GetUserByIdAsync(category.CreatedById, cancellationToken);
            }

            ReadCategoryDto dto = mapper.Map<ReadCategoryDto>(category, opt => opt.Items["CreatedBy"] = createdBy);

            return dto;
        }
    }
}
