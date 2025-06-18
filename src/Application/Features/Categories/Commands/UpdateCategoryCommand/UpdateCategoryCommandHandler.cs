using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Categories.Commands.UpdateCategoryCommand
{
    public class UpdateCategoryCommandHandler(IAppDbContext context) : IRequestHandler<UpdateCategoryCommand, Result>
    {
        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken = default)
        {
            Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (category == null)
            {
                return CategoriesErrors.NotFound(request.CategoryId);
            }

            if (context.Categories.Any(c => c.Name == request.Name && c.Id != request.CategoryId))
            {
                return CategoriesErrors.Conflict("name");
            }

            category.Name = request.Name;
            category.Description = request.Description;

            _ = context.Categories.Update(category);

            _ = await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
