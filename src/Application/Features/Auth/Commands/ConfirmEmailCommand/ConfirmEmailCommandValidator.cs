using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.ConfirmEmailCommand
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            _ = RuleFor(x => x.UserId).NotEmpty().WithErrorCode(ValidationCodes.Required);

            _ = RuleFor(x => x.Token).NotEmpty().WithErrorCode(ValidationCodes.Required);
        }
    }
}
