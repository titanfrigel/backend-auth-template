using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetAllCategoriesQuery
{
    public class GetAllCategoriesQueryValidator : AbstractValidator<GetAllCategoriesQuery>
    {
        public GetAllCategoriesQueryValidator()
        {
            _ = RuleFor(x => x).NotNull().WithErrorCode(ValidationCodes.Required);

            _ = RuleFor(x => x.Include).NotNull().WithErrorCode(ValidationCodes.Required);
        }
    }
}
