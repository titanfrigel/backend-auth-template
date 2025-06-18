using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoryByIdQuery;
using BackendAuthTemplate.Tests.Common.Categories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Validators
{
    public class GetCategoryByIdQueryValidatorFunctionalTests
    {
        private readonly GetCategoryByIdQueryValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_CategoryId_Is_Empty()
        {
            GetCategoryByIdQuery query = CategoriesQueriesTestHelper.GetCategoryByIdQuery(id: Guid.Empty);

            TestValidationResult<GetCategoryByIdQuery> result = _validator.TestValidate(query);

            _ = result.ShouldHaveValidationErrorFor(x => x.CategoryId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Query()
        {
            GetCategoryByIdQuery query = CategoriesQueriesTestHelper.GetCategoryByIdQuery();

            TestValidationResult<GetCategoryByIdQuery> result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
