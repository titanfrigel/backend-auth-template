using BackendAuthTemplate.Application.Common.Result;

namespace BackendAuthTemplate.Application.Features.Auth
{
    public class AuthErrors : EntityError<AuthErrors>
    {
        protected override string Entity => "Auth";
    }
}
