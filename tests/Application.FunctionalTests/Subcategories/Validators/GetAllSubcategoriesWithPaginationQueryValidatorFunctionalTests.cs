using BackendAuthTemplate.Application.Features.Subcategories.Queries.GetAllSubcategoriesWithPaginationQuery;
using BackendAuthTemplate.Tests.Common.Subcategories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Subcategories.Validators
{
    public class GetAllSubcategoriesWithPaginationQueryValidatorFunctionalTests
    {
        private readonly GetAllSubcategoriesWithPaginationQueryValidator _validator = new();

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Query()
        {
            GetAllSubcategoriesWithPaginationQuery query = SubcategoriesQueriesTestHelper.GetAllSubcategoriesWithPaginationQuery();

            TestValidationResult<GetAllSubcategoriesWithPaginationQuery> result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
