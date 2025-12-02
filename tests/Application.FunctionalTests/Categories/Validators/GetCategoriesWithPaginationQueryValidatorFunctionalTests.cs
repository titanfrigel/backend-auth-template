using BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoriesWithPaginationQuery;
using BackendAuthTemplate.Tests.Common.Categories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Validators
{
    public class GetCategoriesWithPaginationQueryValidatorFunctionalTests
    {
        private readonly GetCategoriesWithPaginationQueryValidator _validator = new();

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Query()
        {
            GetCategoriesWithPaginationQuery query = CategoriesQueriesTestHelper.GetCategoriesWithPaginationQuery();

            TestValidationResult<GetCategoriesWithPaginationQuery> result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
