using AutoMapper;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetAllSubcategoriesQuery
{
    public class GetAllSubcategoriesQueryHandler(IAppDbContext context, IMapper mapper, IUser userContext, IIdentityService identityService, IIncludeConfigurator<Subcategory> includeConfigurator) : IRequestHandler<GetAllSubcategoriesQuery, Result<List<ReadSubcategoryDto>>>
    {
        public async Task<Result<List<ReadSubcategoryDto>>> Handle(GetAllSubcategoriesQuery request, CancellationToken cancellationToken = default)
        {
            IncludeBuilder<Subcategory> builder = new(includeConfigurator, request.Include, userContext.Roles);
            IQueryable<Subcategory> query = builder.Apply(context.Subcategories.AsNoTracking());

            List<Subcategory> subcategories = await query.ToListAsync(cancellationToken);

            List<ReadSubcategoryDto> dtos = [];

            foreach (Subcategory subcategory in subcategories)
            {
                ReadUserDto? createdBy = null;

                if (builder.tree.ContainsKey("createdBy"))
                {
                    createdBy = await identityService.GetUserByIdAsync(subcategory.CreatedById, cancellationToken);
                }

                ReadSubcategoryDto dto = mapper.Map<ReadSubcategoryDto>(subcategory, opt => opt.Items["CreatedBy"] = createdBy);

                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
