using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoriesWithPaginationQuery
{
    public class GetCategoriesWithPaginationQueryValidator : AbstractValidator<GetCategoriesWithPaginationQuery>
    {
        public GetCategoriesWithPaginationQueryValidator()
        {
            _ = RuleFor(x => x).NotNull().WithErrorCode(ValidationCodes.Required);

            _ = RuleFor(x => x.PageNumber).GreaterThan(0).WithErrorCode(ValidationCodes.TooSmall);

            _ = RuleFor(x => x.PageSize)
                .GreaterThan(0).WithErrorCode(ValidationCodes.TooSmall)
                .LessThanOrEqualTo(100).WithErrorCode(ValidationCodes.TooLarge);
        }
    }
}
