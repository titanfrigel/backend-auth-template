using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Subcategories.Commands.CreateSubcategoryCommand
{
    public class CreateSubcategoryCommandValidator : AbstractValidator<CreateSubcategoryCommand>
    {
        public CreateSubcategoryCommandValidator()
        {
            _ = RuleFor(x => x.Name)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MinimumLength(3).WithErrorCode(ValidationCodes.TooShort)
                .MaximumLength(100).WithErrorCode(ValidationCodes.TooLong);

            _ = RuleFor(x => x.Description)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MinimumLength(3).WithErrorCode(ValidationCodes.TooShort)
                .MaximumLength(1000).WithErrorCode(ValidationCodes.TooLong);

            _ = RuleFor(x => x.CategoryId)
                .NotEmpty().WithErrorCode(ValidationCodes.Required);
        }
    }
}
