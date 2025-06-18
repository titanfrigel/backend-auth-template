using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Subcategories.Commands.UpdateSubcategoryCommand
{
    public class UpdateSubcategoryCommand : IRequest<Result>
    {
        public required Guid SubcategoryId { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required Guid CategoryId { get; init; }
    }
}
