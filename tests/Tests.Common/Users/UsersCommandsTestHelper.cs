using BackendAuthTemplate.Application.Features.Users.Commands.UpdateUserMeCommand;

namespace BackendAuthTemplate.Tests.Common.Users
{
    public static class UsersCommandsTestHelper
    {
        public static UpdateUserMeCommand UpdateUserMeCommand(
            string phoneNumber = "+33666666666",
            string firstName = "Tom",
            string lastName = "Etnana",
            string address = "12 Rue de la Rue",
            string city = "Nancy",
            string zipCode = "54000",
            string countryCode = "FRA"
        )
        {
            return new()
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                City = city,
                ZipCode = zipCode,
                CountryCode = countryCode
            };
        }
    }
}
