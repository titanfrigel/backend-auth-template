using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.FeatureName.Queries.GetAllFeatureNameWithPaginationQuery
{
    public class GetAllFeatureNameWithPaginationQuery : IRequest<Result<PaginatedList<ReadEntityNameDto>>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public List<string> Include { get; init; } = [];
    }
}
