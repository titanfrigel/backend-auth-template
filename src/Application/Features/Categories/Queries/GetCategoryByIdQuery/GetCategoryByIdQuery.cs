using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoryByIdQuery
{
    public class GetCategoryByIdQuery : IRequest<Result<ReadCategoryDto>>, IIncludable
    {
        public required Guid CategoryId { get; init; }
        public IList<string>? Includes { get; init; }
    }
}
