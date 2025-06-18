using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoryByIdQuery
{
    public class GetSubcategoryByIdQueryValidator : AbstractValidator<GetSubcategoryByIdQuery>
    {
        public GetSubcategoryByIdQueryValidator()
        {
            _ = RuleFor(x => x.SubcategoryId).NotEmpty().WithErrorCode(ValidationCodes.Required);
        }
    }
}
