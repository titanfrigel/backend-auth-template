using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.ForgotPasswordCommand
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            _ = RuleFor(x => x.Email)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .EmailAddress().WithErrorCode(ValidationCodes.Invalid);
        }
    }
}
