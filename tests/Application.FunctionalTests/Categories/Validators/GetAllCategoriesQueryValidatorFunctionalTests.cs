using BackendAuthTemplate.Application.Features.Categories.Queries.GetAllCategoriesQuery;
using BackendAuthTemplate.Tests.Common.Categories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Validators
{
    public class GetAllCategoriesQueryValidatorFunctionalTests
    {
        private readonly GetAllCategoriesQueryValidator _validator = new();

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Query()
        {
            GetAllCategoriesQuery query = CategoriesQueriesTestHelper.GetAllCategoriesQuery();

            TestValidationResult<GetAllCategoriesQuery> result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
