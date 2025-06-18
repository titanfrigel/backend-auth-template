using BackendAuthTemplate.Tests.Common.Moqs;

namespace BackendAuthTemplate.Tests.Common.Users
{
    public static class UsersEntitiesTestHelper
    {
        public static FakeAppUser CreateValidUser(
            Guid? id = null,
            string email = "Email",
            string phoneNumber = "PhoneNumber",
            string firstName = "Tom",
            string lastName = "Etnana",
            string address = "Address",
            string city = "City",
            string zipCode = "ZipCode",
            string countryCode = "CountryCode",
            DateTimeOffset? createdAt = null
        )
        {
            return new()
            {
                Id = id ?? Guid.NewGuid(),
                Email = email,
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                City = city,
                ZipCode = zipCode,
                CountryCode = countryCode,
                CreatedAt = createdAt ?? DateTimeOffset.UtcNow
            };
        }
    }
}
