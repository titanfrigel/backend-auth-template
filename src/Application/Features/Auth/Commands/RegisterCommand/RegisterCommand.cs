using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.RegisterCommand
{
    public class RegisterCommand : IRequest<Result>
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
    }
}
