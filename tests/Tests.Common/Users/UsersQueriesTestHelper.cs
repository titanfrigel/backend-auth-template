using BackendAuthTemplate.Application.Features.Users.Queries.GetUserMeQuery;
using BackendAuthTemplate.Application.Features.Users.Queries.GetUserQuery;

namespace BackendAuthTemplate.Tests.Common.Users
{
    public static class UsersQueriesTestHelper
    {
        public static GetUserMeQuery GetUserMeQuery()
        {
            return new()
            {
            };
        }

        public static GetUserQuery GetUserQuery(Guid? userId = null)
        {
            return new()
            {
                UserId = userId ?? Guid.NewGuid()
            };
        }
    }
}
