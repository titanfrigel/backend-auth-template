using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.ResendEmailConfirmationCommand
{
    public class ResendEmailConfirmationCommandValidator : AbstractValidator<ResendEmailConfirmationCommand>
    {
        public ResendEmailConfirmationCommandValidator()
        {
            _ = RuleFor(x => x.Email)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .EmailAddress().WithErrorCode(ValidationCodes.Invalid);
        }
    }
}
