using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetAllCategoriesWithPaginationQuery
{
    public class GetAllCategoriesWithPaginationQuery : IRequest<Result<PaginatedList<ReadCategoryDto>>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public List<string> Include { get; init; } = [];
    }
}