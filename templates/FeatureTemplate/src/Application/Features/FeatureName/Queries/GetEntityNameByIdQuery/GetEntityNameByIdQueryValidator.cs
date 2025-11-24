using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.FeatureName.Queries.GetEntityNameByIdQuery
{
    public class GetEntityNameByIdQueryValidator : AbstractValidator<GetEntityNameByIdQuery>
    {
        public GetEntityNameByIdQueryValidator()
        {
            _ = RuleFor(x => x.EntityNameId).NotEmpty().WithErrorCode(ValidationCodes.Required);

            _ = RuleFor(x => x.Include).NotNull().WithErrorCode(ValidationCodes.Required);
        }
    }
}
