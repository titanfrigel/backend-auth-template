using AutoMapper;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetAllCategoriesQuery
{
    public class GetAllCategoriesQueryHandler(IAppDbContext context, IMapper mapper, IUser userContext, IIdentityService identityService, IIncludeConfigurator<Category> includeConfigurator) : IRequestHandler<GetAllCategoriesQuery, Result<List<ReadCategoryDto>>>
    {
        public async Task<Result<List<ReadCategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken = default)
        {
            IncludeBuilder<Category> builder = new(includeConfigurator, request.Include, userContext.Roles);
            IQueryable<Category> query = builder.Apply(context.Categories.AsNoTracking());

            List<Category> categories = await query.ToListAsync(cancellationToken);

            List<ReadCategoryDto> dtos = [];

            foreach (Category category in categories)
            {
                ReadUserDto? createdBy = null;

                if (builder.tree.ContainsKey("createdBy"))
                {
                    createdBy = await identityService.GetUserByIdAsync(category.CreatedById, cancellationToken);
                }

                ReadCategoryDto dto = mapper.Map<ReadCategoryDto>(category, opt => opt.Items["CreatedBy"] = createdBy);

                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
