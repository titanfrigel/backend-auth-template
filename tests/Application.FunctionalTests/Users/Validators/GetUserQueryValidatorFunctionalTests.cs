using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Users.Queries.GetUserQuery;
using BackendAuthTemplate.Tests.Common.Users;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Users.Validators
{
    public class GetUserQueryValidatorFunctionalTests
    {
        private readonly GetUserQueryValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            GetUserQuery query = UsersQueriesTestHelper.GetUserQuery(userId: Guid.Empty);

            TestValidationResult<GetUserQuery> result = _validator.TestValidate(query);

            _ = result.ShouldHaveValidationErrorFor(x => x.UserId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Query()
        {
            GetUserQuery query = UsersQueriesTestHelper.GetUserQuery();

            TestValidationResult<GetUserQuery> result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
