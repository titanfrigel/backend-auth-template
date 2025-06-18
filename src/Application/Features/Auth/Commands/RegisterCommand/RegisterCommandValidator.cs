using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.RegisterCommand
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            _ = RuleFor(x => x.Email)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .EmailAddress().WithErrorCode(ValidationCodes.Invalid);

            _ = RuleFor(x => x.Password)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MinimumLength(8).WithErrorCode(ValidationCodes.TooShort)
                .MaximumLength(100).WithErrorCode(ValidationCodes.TooLong)
                .Matches(@"\d").WithErrorCode(ValidationCodes.Invalid);

            _ = RuleFor(x => x.FirstName)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MinimumLength(2).WithErrorCode(ValidationCodes.TooShort)
                .MaximumLength(100).WithErrorCode(ValidationCodes.TooLong)
                .Matches(@"^[A-Za-zÀ-ÿ'-]+( [A-Za-zÀ-ÿ'-]+)*$").WithErrorCode(ValidationCodes.Invalid);

            _ = RuleFor(x => x.LastName)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MinimumLength(2).WithErrorCode(ValidationCodes.TooShort)
                .MaximumLength(100).WithErrorCode(ValidationCodes.TooLong)
                .Matches(@"^[A-Za-zÀ-ÿ'-]+( [A-Za-zÀ-ÿ'-]+)*$").WithErrorCode(ValidationCodes.Invalid);

            _ = RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MaximumLength(20).WithErrorCode(ValidationCodes.TooLong)
                .PhoneNumber().WithErrorCode(ValidationCodes.Invalid);

            _ = RuleFor(x => x.Address)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MinimumLength(5).WithMessage(ValidationCodes.TooShort)
                .MaximumLength(200).WithErrorCode(ValidationCodes.TooLong);

            _ = RuleFor(x => x.City)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MaximumLength(100).WithErrorCode(ValidationCodes.TooLong);

            _ = RuleFor(x => x.ZipCode)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MaximumLength(20).WithErrorCode(ValidationCodes.TooLong)
                .Matches(@"^[0-9A-Za-z\- ]+$").WithErrorCode(ValidationCodes.Invalid);

            _ = RuleFor(x => x.CountryCode)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .CountryCode().WithErrorCode(ValidationCodes.Invalid);
        }
    }
}
