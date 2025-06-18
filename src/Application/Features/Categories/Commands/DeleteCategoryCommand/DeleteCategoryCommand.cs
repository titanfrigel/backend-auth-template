using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Categories.Commands.DeleteCategoryCommand
{
    public class DeleteCategoryCommand : IRequest<Result>
    {
        public required Guid CategoryId { get; init; }
    }
}
