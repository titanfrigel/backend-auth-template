using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoryByIdQuery
{
    public class GetCategoryByIdQuery : IRequest<Result<ReadCategoryDto>>
    {
        public required Guid CategoryId { get; init; }
        public List<string> Include { get; init; } = [];
    }
}
