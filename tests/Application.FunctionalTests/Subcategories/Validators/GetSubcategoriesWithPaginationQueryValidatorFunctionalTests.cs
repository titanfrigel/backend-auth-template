using BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoriesWithPaginationQuery;
using BackendAuthTemplate.Tests.Common.Subcategories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Subcategories.Validators
{
    public class GetSubcategoriesWithPaginationQueryValidatorFunctionalTests
    {
        private readonly GetSubcategoriesWithPaginationQueryValidator _validator = new();

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Query()
        {
            GetSubcategoriesWithPaginationQuery query = SubcategoriesQueriesTestHelper.GetSubcategoriesWithPaginationQuery();

            TestValidationResult<GetSubcategoriesWithPaginationQuery> result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
