using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Subcategories.Commands.CreateSubcategoryCommand
{
    public class CreateSubcategoryCommandHandler(IAppDbContext context) : IRequestHandler<CreateSubcategoryCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateSubcategoryCommand request, CancellationToken cancellationToken = default)
        {
            Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (category == null)
            {
                return CategoriesErrors.NotFound(request.CategoryId);
            }

            if (context.Subcategories.Any(c => c.Name == request.Name))
            {
                return SubcategoriesErrors.Conflict("name");
            }

            Subcategory subcategory = new() { Name = request.Name, Description = request.Description, CategoryId = request.CategoryId };

            subcategory.AddDomainEvent(new SubcategoryCreatedEvent(subcategory.Id));

            _ = context.Subcategories.Add(subcategory);

            _ = await context.SaveChangesAsync(cancellationToken);

            return subcategory.Id;
        }
    }
}
