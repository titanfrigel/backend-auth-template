using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoriesWithPaginationQuery
{
    public class GetSubcategoriesWithPaginationQuery : IRequest<Result<PaginatedList<ReadSubcategoryDto>>>, IPaginable, IIncludable, ISortable
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public IList<string>? Includes { get; init; }
        public IList<Sort>? Sorts { get; init; }
    }
}