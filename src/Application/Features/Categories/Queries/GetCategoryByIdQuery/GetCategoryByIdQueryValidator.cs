using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoryByIdQuery
{
    public class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
    {
        public GetCategoryByIdQueryValidator()
        {
            _ = RuleFor(x => x.CategoryId).NotEmpty().WithErrorCode(ValidationCodes.Required);

            _ = RuleFor(x => x.Include).NotNull().WithErrorCode(ValidationCodes.Required);
        }
    }
}
