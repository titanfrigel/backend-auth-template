using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoryByIdQuery
{
    public class GetSubcategoryByIdQueryHandler(IAppDbContext context, IMapper mapper, IUser userContext, IIncludeConfigurator<Subcategory> includeConfigurator) : IRequestHandler<GetSubcategoryByIdQuery, Result<ReadSubcategoryDto>>
    {
        public async Task<Result<ReadSubcategoryDto>> Handle(GetSubcategoryByIdQuery request, CancellationToken cancellationToken = default)
        {
            IncludeBuilder<Subcategory> builder = new(includeConfigurator, request.Include, userContext.Roles);
            IQueryable<Subcategory> query = builder.Apply(context.Subcategories.AsNoTracking());

            ReadSubcategoryDto? dto = await query
                .Where(c => c.Id == request.SubcategoryId)
                .ProjectTo<ReadSubcategoryDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
            {
                return SubcategoriesErrors.NotFound(request.SubcategoryId);
            }

            return dto;
        }
    }
}
