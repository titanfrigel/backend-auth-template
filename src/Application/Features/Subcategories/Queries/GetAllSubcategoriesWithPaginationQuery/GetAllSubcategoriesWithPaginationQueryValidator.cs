using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetAllSubcategoriesWithPaginationQuery
{
    public class GetAllSubcategoriesWithPaginationQueryValidator : AbstractValidator<GetAllSubcategoriesWithPaginationQuery>
    {
        public GetAllSubcategoriesWithPaginationQueryValidator()
        {
            _ = RuleFor(x => x).NotNull().WithErrorCode(ValidationCodes.Required);
        }
    }
}
