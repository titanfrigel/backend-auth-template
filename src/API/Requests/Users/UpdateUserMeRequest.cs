using AutoMapper;
using BackendAuthTemplate.Application.Features.Users.Commands.UpdateUserMeCommand;

namespace BackendAuthTemplate.API.Requests.Users
{
    public class UpdateUserMeRequest
    {
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
                _ = CreateMap<UpdateUserMeRequest, UpdateUserMeCommand>();
            }
        }
    }
}
