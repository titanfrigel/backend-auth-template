using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.FeatureName.Queries.GetEntityNameByIdQuery
{
    public class GetEntityNameByIdQuery : IRequest<Result<ReadEntityNameDto>>
    {
        public required Guid EntityNameId { get; init; }
        public List<string> Include { get; init; } = [];
    }
}
