using AutoMapper;
using BackendAuthTemplate.Application.Features.Auth.Commands.ResetPasswordCommand;

namespace BackendAuthTemplate.API.Requests.Auth
{
    public class ResetPasswordRequest
    {
        public required Guid UserId { get; init; }
        public required string Token { get; init; }
        public required string NewPassword { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<ResetPasswordRequest, ResetPasswordCommand>();
            }
        }
    }
}
