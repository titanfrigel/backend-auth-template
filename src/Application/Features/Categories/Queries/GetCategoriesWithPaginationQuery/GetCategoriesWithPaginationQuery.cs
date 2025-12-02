using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoriesWithPaginationQuery
{
    public class GetCategoriesWithPaginationQuery : IRequest<Result<PaginatedList<ReadCategoryDto>>>, IPaginable, ISortable, IIncludable
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public IList<string>? Includes { get; init; }
        public IList<Sort>? Sorts { get; init; }
    }
}
