using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.FeatureName.Queries.GetFeatureNameWithPaginationQuery
{
    public class GetFeatureNameWithPaginationQuery : IRequest<Result<PaginatedList<ReadEntityNameDto>>>, IPaginable, IIncludable, ISortable
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public IList<string>? Includes { get; init; }
        public IList<Sort>? Sorts { get; init; }
    }
}
