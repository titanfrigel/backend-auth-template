using BackendAuthTemplate.API.Requests.Users;

namespace BackendAuthTemplate.Tests.Common.Users
{
    public class UsersRequestsTestHelper
    {
        public static UpdateUserMeRequest UpdateUserMeRequest(
            string phoneNumber = "+33666666666",
            string firstName = "John",
            string lastName = "Doe",
            string address = "123 Fake Street",
            string city = "Faketown",
            string zipCode = "12345",
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
