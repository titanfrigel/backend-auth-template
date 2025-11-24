using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.FeatureName.Queries.GetEntityNameByIdQuery;
using BackendAuthTemplate.Tests.Common.FeatureName;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Validators
{
    public class GetEntityNameByIdQueryValidatorFunctionalTests
    {
        private readonly GetEntityNameByIdQueryValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_EntityNameId_Is_Empty()
        {
            GetEntityNameByIdQuery query = FeatureNameQueriesTestHelper.GetEntityNameByIdQuery(id: Guid.Empty);

            TestValidationResult<GetEntityNameByIdQuery> result = _validator.TestValidate(query);

            _ = result.ShouldHaveValidationErrorFor(x => x.EntityNameId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Query()
        {
            GetEntityNameByIdQuery query = FeatureNameQueriesTestHelper.GetEntityNameByIdQuery();

            TestValidationResult<GetEntityNameByIdQuery> result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
