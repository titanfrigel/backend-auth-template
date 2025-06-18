using AutoMapper;
using BackendAuthTemplate.Application.Features.Auth.Commands.ResendEmailConfirmationCommand;

namespace BackendAuthTemplate.API.Requests.Auth
{
    public class ResendEmailConfirmationRequest
    {
        public required string Email { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<ResendEmailConfirmationRequest, ResendEmailConfirmationCommand>();
            }
        }
    }
}
