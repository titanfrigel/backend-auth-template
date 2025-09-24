using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetAllCategoriesQuery
{
    public class GetAllCategoriesQuery : IRequest<Result<List<ReadCategoryDto>>>
    {
        public List<string> Include { get; init; } = [];
    }
}