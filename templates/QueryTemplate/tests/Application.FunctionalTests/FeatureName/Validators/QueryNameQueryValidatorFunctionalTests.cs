using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.FeatureName.Queries.QueryNameQuery;
using BackendAuthTemplate.Tests.Common.FeatureName;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Validators
{
    public class QueryNameQueryValidatorFunctionalTests
    {
        private readonly QueryNameQueryValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty()
        {
            QueryNameQuery query = FeatureNameQueriesTestHelper.QueryNameQuery(id: Guid.Empty);

            TestValidationResult<QueryNameQuery> result = _validator.TestValidate(query);

            _ = result.ShouldHaveValidationErrorFor(x => x.EntityNameId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Query()
        {
            QueryNameQuery query = FeatureNameQueriesTestHelper.QueryNameQuery();

            TestValidationResult<QueryNameQuery> result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
