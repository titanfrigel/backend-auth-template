using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.FeatureName.Queries.QueryNameQuery
{
    public class QueryNameQuery : IRequest<Result<ReadEntityNameDto>>
    {
        public required Guid EntityNameId { get; init; }
    }
}
