using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Application.Features.Users.Queries.GetUserQuery;
using BackendAuthTemplate.Tests.Common.Users;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Users.Queries
{
    public class GetUserQueryFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task GetMeQuery_Should_Fail()
        {
            GetUserQuery query = UsersQueriesTestHelper.GetUserQuery();

            Result<ReadUserDto> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeFalse();
        }
    }
}
