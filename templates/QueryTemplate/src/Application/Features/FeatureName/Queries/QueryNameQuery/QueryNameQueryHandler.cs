using AutoMapper;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.FeatureName.Queries.QueryNameQuery
{
    public class QueryNameQueryHandler(IAppDbContext context, IMapper mapper) : IRequestHandler<QueryNameQuery, Result<ReadEntityNameDto>>
    {
        public async Task<Result<ReadEntityNameDto>> Handle(QueryNameQuery request, CancellationToken cancellationToken = default)
        {
            EntityName? entityName = await context.FeatureName
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.EntityNameId, cancellationToken);

            if (entityName == null)
            {
                return FeatureNameErrors.NotFound(request.EntityNameId);
            }

            return mapper.Map<ReadEntityNameDto>(entityName);
        }
    }
}
