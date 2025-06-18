using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoryByIdQuery;
using BackendAuthTemplate.Tests.Common.Subcategories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Subcategories.Validators
{
    public class GetSubcategoryByIdQueryValidatorFunctionalTests
    {
        private readonly GetSubcategoryByIdQueryValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_SubcategoryId_Is_Empty()
        {
            GetSubcategoryByIdQuery query = SubcategoriesQueriesTestHelper.GetSubcategoryByIdQuery(id: Guid.Empty);

            TestValidationResult<GetSubcategoryByIdQuery> result = _validator.TestValidate(query);

            _ = result.ShouldHaveValidationErrorFor(x => x.SubcategoryId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Query()
        {
            GetSubcategoryByIdQuery query = SubcategoriesQueriesTestHelper.GetSubcategoryByIdQuery();

            TestValidationResult<GetSubcategoryByIdQuery> result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
