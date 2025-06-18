using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Subcategories.Commands.DeleteSubcategoryCommand
{
    public class DeleteSubcategoryCommandValidator : AbstractValidator<DeleteSubcategoryCommand>
    {
        public DeleteSubcategoryCommandValidator()
        {
            _ = RuleFor(x => x.SubcategoryId).NotEmpty().WithErrorCode(ValidationCodes.Required);
        }
    }
}
