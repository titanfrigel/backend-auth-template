using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.RefreshTokenCommand
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            _ = RuleFor(x => x).NotNull().WithErrorCode(ValidationCodes.Required);
        }
    }
}
