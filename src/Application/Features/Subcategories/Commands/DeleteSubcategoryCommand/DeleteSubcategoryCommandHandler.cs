using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Subcategories.Commands.DeleteSubcategoryCommand
{
    public class DeleteSubcategoryCommandHandler(IAppDbContext context) : IRequestHandler<DeleteSubcategoryCommand, Result>
    {
        public async Task<Result> Handle(DeleteSubcategoryCommand request, CancellationToken cancellationToken = default)
        {
            Subcategory? subcategory = await context.Subcategories
                .FirstOrDefaultAsync(c => c.Id == request.SubcategoryId, cancellationToken);

            if (subcategory == null)
            {
                return SubcategoriesErrors.NotFound(request.SubcategoryId);
            }

            _ = context.Subcategories.Remove(subcategory);

            _ = await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
