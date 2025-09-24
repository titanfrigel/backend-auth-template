using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetAllSubcategoriesQuery
{
    public class GetAllSubcategoriesQueryValidator : AbstractValidator<GetAllSubcategoriesQuery>
    {
        public GetAllSubcategoriesQueryValidator()
        {
            _ = RuleFor(x => x).NotNull().WithErrorCode(ValidationCodes.Required);

            _ = RuleFor(x => x.Include).NotNull().WithErrorCode(ValidationCodes.Required);
        }
    }
}
