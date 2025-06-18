using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Subcategories.Commands.CreateSubcategoryCommand
{
    public class CreateSubcategoryCommand : IRequest<Result<Guid>>
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required Guid CategoryId { get; init; }
    }
}
