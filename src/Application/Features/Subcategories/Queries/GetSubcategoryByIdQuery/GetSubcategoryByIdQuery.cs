using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoryByIdQuery
{
    public class GetSubcategoryByIdQuery : IRequest<Result<ReadSubcategoryDto>>
    {
        public required Guid SubcategoryId { get; init; }
        public List<string> Include { get; init; } = [];
    }
}
