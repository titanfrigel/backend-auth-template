using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoriesWithPaginationQuery
{
    public class GetSubcategoriesWithPaginationQueryValidator : AbstractValidator<GetSubcategoriesWithPaginationQuery>
    {
        public GetSubcategoriesWithPaginationQueryValidator()
        {
            _ = RuleFor(x => x).NotNull().WithErrorCode(ValidationCodes.Required);

            _ = RuleFor(x => x.PageNumber).GreaterThan(0).WithErrorCode(ValidationCodes.TooSmall);

            _ = RuleFor(x => x.PageSize)
                .GreaterThan(0).WithErrorCode(ValidationCodes.TooSmall)
                .LessThanOrEqualTo(100).WithErrorCode(ValidationCodes.TooLarge);
        }
    }
}
