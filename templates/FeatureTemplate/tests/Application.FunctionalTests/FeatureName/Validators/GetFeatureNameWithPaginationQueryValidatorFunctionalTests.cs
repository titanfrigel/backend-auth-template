using BackendAuthTemplate.Application.Features.FeatureName.Queries.GetFeatureNameWithPaginationQuery;
using BackendAuthTemplate.Tests.Common.FeatureName;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Validators
{
    public class GetFeatureNameWithPaginationQueryValidatorFunctionalTests
    {
        private readonly GetFeatureNameWithPaginationQueryValidator _validator = new();

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Query()
        {
            GetFeatureNameWithPaginationQuery query = FeatureNameQueriesTestHelper.GetFeatureNameWithPaginationQuery();

            TestValidationResult<GetFeatureNameWithPaginationQuery> result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
