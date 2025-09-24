using BackendAuthTemplate.Application.Features.Categories.Queries.GetAllCategoriesWithPaginationQuery;
using BackendAuthTemplate.Tests.Common.Categories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Validators
{
    public class GetAllCategoriesWithPaginationQueryValidatorFunctionalTests
    {
        private readonly GetAllCategoriesWithPaginationQueryValidator _validator = new();

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Query()
        {
            GetAllCategoriesWithPaginationQuery query = CategoriesQueriesTestHelper.GetAllCategoriesWithPaginationQuery();

            TestValidationResult<GetAllCategoriesWithPaginationQuery> result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
