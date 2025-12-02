using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoryByIdQuery
{
    public class GetSubcategoryByIdQuery : IRequest<Result<ReadSubcategoryDto>>, IIncludable
    {
        public required Guid SubcategoryId { get; init; }
        public IList<string>? Includes { get; init; }
    }
}
