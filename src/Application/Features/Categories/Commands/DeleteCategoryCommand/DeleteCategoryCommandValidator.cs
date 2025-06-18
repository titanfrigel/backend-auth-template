using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Categories.Commands.DeleteCategoryCommand
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            _ = RuleFor(x => x.CategoryId).NotEmpty().WithErrorCode(ValidationCodes.Required);
        }
    }
}
