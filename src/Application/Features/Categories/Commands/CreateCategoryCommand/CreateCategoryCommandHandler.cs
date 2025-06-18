using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Domain.Events;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Categories.Commands.CreateCategoryCommand
{
    public class CreateCategoryCommandHandler(IAppDbContext context) : IRequestHandler<CreateCategoryCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken = default)
        {
            if (context.Categories.Any(c => c.Name == request.Name))
            {
                return CategoriesErrors.Conflict("name");
            }

            Category category = new() { Name = request.Name, Description = request.Description };

            category.AddDomainEvent(new CategoryCreatedEvent(category.Id));

            _ = context.Categories.Add(category);

            _ = await context.SaveChangesAsync(cancellationToken);

            return category.Id;
        }
    }
}
