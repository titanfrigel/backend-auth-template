using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetAllCategoriesWithPaginationQuery
{
    public class GetAllCategoriesWithPaginationQueryValidator : AbstractValidator<GetAllCategoriesWithPaginationQuery>
    {
        public GetAllCategoriesWithPaginationQueryValidator()
        {
            _ = RuleFor(x => x).NotNull().WithErrorCode(ValidationCodes.Required);
        }
    }
}
