using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.FeatureName.Commands.CommandNameCommand
{
    public class CommandNameCommandValidator : AbstractValidator<CommandNameCommand>
    {
        public CommandNameCommandValidator()
        {
            _ = RuleFor(x => x.Name)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MinimumLength(3).WithErrorCode(ValidationCodes.TooShort)
                .MaximumLength(100).WithErrorCode(ValidationCodes.TooLong);
        }
    }
}
