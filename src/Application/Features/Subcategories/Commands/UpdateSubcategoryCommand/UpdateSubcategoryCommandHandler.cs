using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Subcategories.Commands.UpdateSubcategoryCommand
{
    public class UpdateSubcategoryCommandHandler(IAppDbContext context) : IRequestHandler<UpdateSubcategoryCommand, Result>
    {
        public async Task<Result> Handle(UpdateSubcategoryCommand request, CancellationToken cancellationToken = default)
        {
            Subcategory? subcategory = await context.Subcategories.FirstOrDefaultAsync(c => c.Id == request.SubcategoryId, cancellationToken);

            if (subcategory == null)
            {
                return SubcategoriesErrors.NotFound(request.SubcategoryId);
            }

            Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (category == null)
            {
                return CategoriesErrors.NotFound(request.CategoryId);
            }

            if (context.Subcategories.Any(c => c.Name == request.Name && c.Id != request.SubcategoryId))
            {
                _ = SubcategoriesErrors.Conflict("name");
            }

            subcategory.Name = request.Name;
            subcategory.Description = request.Description;
            subcategory.CategoryId = request.CategoryId;

            _ = context.Subcategories.Update(subcategory);

            _ = await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
