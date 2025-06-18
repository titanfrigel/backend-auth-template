using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.LoginCommand
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            _ = RuleFor(x => x.Email)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .EmailAddress().WithErrorCode(ValidationCodes.Invalid);

            _ = RuleFor(x => x.Password)
                .NotEmpty().WithErrorCode(ValidationCodes.Required);
        }
    }
}
