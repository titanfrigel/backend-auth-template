using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.ResetPasswordCommand
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            _ = RuleFor(x => x.UserId).NotEmpty().WithErrorCode(ValidationCodes.Required);

            _ = RuleFor(x => x.Token).NotEmpty().WithErrorCode(ValidationCodes.Required);

            _ = RuleFor(x => x.NewPassword)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MinimumLength(8).WithErrorCode(ValidationCodes.TooShort)
                .MaximumLength(100).WithErrorCode(ValidationCodes.TooLong)
                .Matches(@"\d").WithErrorCode(ValidationCodes.Invalid);
        }
    }
}
