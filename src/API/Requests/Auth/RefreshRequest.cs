using AutoMapper;
using BackendAuthTemplate.Application.Features.Auth.Commands.RefreshTokenCommand;

namespace BackendAuthTemplate.API.Requests.Auth
{
    public class RefreshTokenRequest
    {
        public string? RefreshToken { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<RefreshTokenRequest, RefreshTokenCommand>();
            }
        }
    }
}
