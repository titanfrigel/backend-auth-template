using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.FeatureName.Commands.DeleteEntityNameCommand
{
    public class DeleteEntityNameCommandValidator : AbstractValidator<DeleteEntityNameCommand>
    {
        public DeleteEntityNameCommandValidator()
        {
            _ = RuleFor(x => x.EntityNameId).NotEmpty().WithErrorCode(ValidationCodes.Required);
        }
    }
}
