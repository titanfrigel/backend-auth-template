using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Categories.Commands.DeleteCategoryCommand
{
    public class DeleteCategoryCommandHandler(IAppDbContext context) : IRequestHandler<DeleteCategoryCommand, Result>
    {
        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken = default)
        {
            Category? category = await context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (category == null)
            {
                return CategoriesErrors.NotFound(request.CategoryId);
            }

            _ = context.Categories.Remove(category);

            _ = await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
