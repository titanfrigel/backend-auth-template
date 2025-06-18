using AutoMapper;
using BackendAuthTemplate.Application.Features.Auth.Commands.ConfirmEmailCommand;

namespace BackendAuthTemplate.API.Requests.Auth
{
    public class ConfirmEmailRequest
    {
        public required Guid UserId { get; init; }
        public required string Token { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<ConfirmEmailRequest, ConfirmEmailCommand>();
            }
        }
    }
}
