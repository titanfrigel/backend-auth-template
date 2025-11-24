using BackendAuthTemplate.Application.Features.FeatureName.Queries.GetAllFeatureNameWithPaginationQuery;
using BackendAuthTemplate.Tests.Common.FeatureName;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Validators
{
    public class GetAllFeatureNameWithPaginationQueryValidatorFunctionalTests
    {
        private readonly GetAllFeatureNameWithPaginationQueryValidator _validator = new();

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Query()
        {
            GetAllFeatureNameWithPaginationQuery query = FeatureNameQueriesTestHelper.GetAllFeatureNameWithPaginationQuery();

            TestValidationResult<GetAllFeatureNameWithPaginationQuery> result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
