using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Application.Features.Users.Queries.GetUserMeQuery;
using BackendAuthTemplate.Tests.Common.Users;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Users.Queries
{
    public class GetUserMeQueryFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task GetMeQuery_Should_Fail()
        {
            GetUserMeQuery query = UsersQueriesTestHelper.GetUserMeQuery();

            Result<ReadUserDto> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeFalse();
        }
    }
}
