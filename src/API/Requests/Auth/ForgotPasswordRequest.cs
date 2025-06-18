using AutoMapper;
using BackendAuthTemplate.Application.Features.Auth.Commands.ForgotPasswordCommand;

namespace BackendAuthTemplate.API.Requests.Auth
{
    public class ForgotPasswordRequest
    {
        public required string Email { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<ForgotPasswordRequest, ForgotPasswordCommand>();
            }
        }
    }
}
