using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.FeatureName.Commands.CreateEntityNameCommand
{
    public class CreateEntityNameCommandValidator : AbstractValidator<CreateEntityNameCommand>
    {
        public CreateEntityNameCommandValidator()
        {
            _ = RuleFor(x => x.Name)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MinimumLength(3).WithErrorCode(ValidationCodes.TooShort)
                .MaximumLength(100).WithErrorCode(ValidationCodes.TooLong);
        }
    }
}
