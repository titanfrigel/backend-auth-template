using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.FeatureName.Queries.GetEntityNameByIdQuery
{
    public class GetEntityNameByIdQuery : IRequest<Result<ReadEntityNameDto>>, IIncludable
    {
        public required Guid EntityNameId { get; init; }
        public IList<string>? Includes { get; init; }
    }
}
