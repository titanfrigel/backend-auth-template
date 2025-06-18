using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Categories.Commands.CreateCategoryCommand
{
    public class CreateCategoryCommand : IRequest<Result<Guid>>
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
    }
}
