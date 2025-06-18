using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoryByIdQuery
{
    public class GetCategoryByIdQueryHandler(IAppDbContext context, IMapper mapper, IUser userContext, IIncludeConfigurator<Category> includeConfigurator) : IRequestHandler<GetCategoryByIdQuery, Result<ReadCategoryDto>>
    {
        public async Task<Result<ReadCategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken = default)
        {
            IncludeBuilder<Category> builder = new(includeConfigurator, request.Include, userContext.Roles);
            IQueryable<Category> query = builder.Apply(context.Categories.AsNoTracking());

            ReadCategoryDto? dto = await query
                .Where(c => c.Id == request.CategoryId)
                .ProjectTo<ReadCategoryDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
            {
                return CategoriesErrors.NotFound(request.CategoryId);
            }

            return dto;
        }
    }
}
