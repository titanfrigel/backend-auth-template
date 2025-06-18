using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetAllSubcategoriesWithPaginationQuery
{
    public class GetAllSubcategoriesWithPaginationQuery : IRequest<Result<PaginatedList<ReadSubcategoryDto>>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public List<string> Include { get; init; } = [];
    }
}