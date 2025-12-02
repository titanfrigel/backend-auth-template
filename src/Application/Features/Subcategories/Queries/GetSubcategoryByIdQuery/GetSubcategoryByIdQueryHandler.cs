using AutoMapper;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoryByIdQuery
{
    public class GetSubcategoryByIdQueryHandler(IAppDbContext context, IMapper mapper, IIdentityService identityService, IUser userContext, IIncludeConfigurator<Subcategory> includeConfigurator) : IRequestHandler<GetSubcategoryByIdQuery, Result<ReadSubcategoryDto>>
    {
        public async Task<Result<ReadSubcategoryDto>> Handle(GetSubcategoryByIdQuery request, CancellationToken cancellationToken = default)
        {
            IQueryable<Subcategory> query = context.Subcategories.AsNoTracking()
                .ApplyIncludes(request, includeConfigurator, userContext, out HashSet<string>? validatedIncludes);

            Subcategory? subcategory = await query
                .Where(c => c.Id == request.SubcategoryId)
                .FirstOrDefaultAsync(cancellationToken);

            if (subcategory == null)
            {
                return SubcategoriesErrors.NotFound(request.SubcategoryId);
            }

            ReadUserDto? createdBy = null;
            if (validatedIncludes.Contains("CreatedBy"))
            {
                createdBy = await identityService.GetUserByIdAsync(subcategory.CreatedById, cancellationToken);
            }

            ReadSubcategoryDto dto = mapper.Map<ReadSubcategoryDto>(subcategory, opt => opt.Items["CreatedBy"] = createdBy);

            return dto;
        }
    }
}
