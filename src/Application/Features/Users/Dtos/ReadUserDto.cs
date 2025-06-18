using AutoMapper;
using BackendAuthTemplate.Domain.Interfaces;

namespace BackendAuthTemplate.Application.Features.Users.Dtos
{
    public class ReadUserDto
    {
        public required Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required string PhoneNumber { get; init; }
        public required string Address { get; init; }
        public required string City { get; init; }
        public required string ZipCode { get; init; }
        public required string CountryCode { get; init; }
        public required DateTimeOffset CreatedAt { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                _ = CreateMap<IAppUser, ReadUserDto>();
            }
        }
    }
}
