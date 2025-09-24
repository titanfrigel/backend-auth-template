using BackendAuthTemplate.Application.Features.Subcategories.Queries.GetAllSubcategoriesQuery;
using BackendAuthTemplate.Tests.Common.Subcategories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Subcategories.Validators
{
    public class GetAllSubcategoriesQueryValidatorFunctionalTests
    {
        private readonly GetAllSubcategoriesQueryValidator _validator = new();

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Query()
        {
            GetAllSubcategoriesQuery query = SubcategoriesQueriesTestHelper.GetAllSubcategoriesQuery();

            TestValidationResult<GetAllSubcategoriesQuery> result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
