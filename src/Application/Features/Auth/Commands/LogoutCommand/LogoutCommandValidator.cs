using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.LogoutCommand
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            _ = RuleFor(x => x).NotNull().WithErrorCode(ValidationCodes.Required);
        }
    }
}
