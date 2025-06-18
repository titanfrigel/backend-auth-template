using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Subcategories.Commands.DeleteSubcategoryCommand
{
    public class DeleteSubcategoryCommand : IRequest<Result>
    {
        public required Guid SubcategoryId { get; init; }
    }
}