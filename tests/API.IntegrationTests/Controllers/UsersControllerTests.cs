using BackendAuthTemplate.Application.Features.Users.Dtos;
using Shouldly;
using System.Net.Http.Json;

namespace BackendAuthTemplate.API.IntegrationTests.Controllers
{
    public class UsersControllerTests : ApiTestBase
    {
        [Fact]
        public async Task GetMe_Should_Return_Me()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/v1/users/me");

            _ = response.EnsureSuccessStatusCode();

            ReadUserDto? user = await response.Content.ReadFromJsonAsync<ReadUserDto>();

            _ = user.ShouldNotBeNull();
            user.Email.ShouldBe("user@example.com");
        }
    }
}
