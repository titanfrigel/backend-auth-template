using BackendAuthTemplate.Application.Common.Result;

namespace BackendAuthTemplate.Application.Features.Users
{
    public class UsersErrors : EntityError<UsersErrors>
    {
        protected override string Entity => "User";
    }
}
