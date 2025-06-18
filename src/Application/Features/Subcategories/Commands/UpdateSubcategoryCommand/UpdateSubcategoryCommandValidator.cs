using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Subcategories.Commands.UpdateSubcategoryCommand
{
    public class UpdateSubcategoryCommandValidator : AbstractValidator<UpdateSubcategoryCommand>
    {
        public UpdateSubcategoryCommandValidator()
        {
            _ = RuleFor(x => x.SubcategoryId).NotEmpty().WithErrorCode(ValidationCodes.Required);

            _ = RuleFor(x => x.Name)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MinimumLength(3).WithErrorCode(ValidationCodes.TooShort)
                .MaximumLength(100).WithErrorCode(ValidationCodes.TooLong);

            _ = RuleFor(x => x.Description)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MinimumLength(3).WithErrorCode(ValidationCodes.TooShort)
                .MaximumLength(1000).WithErrorCode(ValidationCodes.TooLong);

            _ = RuleFor(x => x.CategoryId).NotEmpty().WithErrorCode(ValidationCodes.Required);
        }
    }
}
