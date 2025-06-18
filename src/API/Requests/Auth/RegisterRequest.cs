using AutoMapper;
using BackendAuthTemplate.Application.Features.Auth.Commands.RegisterCommand;

namespace BackendAuthTemplate.API.Requests.Auth
{
    public class RegisterRequest
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string PhoneNumber { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Address { get; init; }
        public required string City { get; init; }
        public required string ZipCode { get; init; }
        public required string CountryCode { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<RegisterRequest, RegisterCommand>();
            }
        }
    }
}
