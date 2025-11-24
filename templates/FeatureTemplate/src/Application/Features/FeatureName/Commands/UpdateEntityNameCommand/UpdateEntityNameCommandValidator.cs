using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.FeatureName.Commands.UpdateEntityNameCommand
{
    public class UpdateEntityNameCommandValidator : AbstractValidator<UpdateEntityNameCommand>
    {
        public UpdateEntityNameCommandValidator()
        {
            _ = RuleFor(x => x.EntityNameId).NotEmpty().WithErrorCode(ValidationCodes.Required);

            _ = RuleFor(x => x.Name)
                .NotEmpty().WithErrorCode(ValidationCodes.Required)
                .MinimumLength(3).WithErrorCode(ValidationCodes.TooShort)
                .MaximumLength(100).WithErrorCode(ValidationCodes.TooLong);
        }
    }
}
