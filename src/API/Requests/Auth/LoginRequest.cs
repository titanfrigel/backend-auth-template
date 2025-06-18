using AutoMapper;
using BackendAuthTemplate.Application.Features.Auth.Commands.LoginCommand;

namespace BackendAuthTemplate.API.Requests.Auth
{
    public class LoginRequest
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required bool RememberMe { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<LoginRequest, LoginCommand>();
            }
        }
    }
}
