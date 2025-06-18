using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Categories.Commands.UpdateCategoryCommand
{
    public class UpdateCategoryCommand : IRequest<Result>
    {
        public required Guid CategoryId { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
    }
}
