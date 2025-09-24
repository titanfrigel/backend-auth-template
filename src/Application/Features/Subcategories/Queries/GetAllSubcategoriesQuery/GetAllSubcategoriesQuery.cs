using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetAllSubcategoriesQuery
{
    public class GetAllSubcategoriesQuery : IRequest<Result<List<ReadSubcategoryDto>>>
    {
        public List<string> Include { get; init; } = [];
    }
}