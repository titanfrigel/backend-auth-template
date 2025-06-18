using BackendAuthTemplate.API.Requests.Auth;
using BackendAuthTemplate.Application.Features.Auth.Dtos;
using BackendAuthTemplate.Tests.Common.Auth;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace BackendAuthTemplate.API.IntegrationTests.Controllers
{
    public class AuthControllerTests : ApiTestBase
    {
        [Fact]
        public async Task Register_Should_Return_Success()
        {
            ClearJwtToken();

            RegisterRequest request = AuthRequestsTestHelper.RegisterRequest();

            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/auth/register", request);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Login_Should_Return_Token()
        {
            ClearJwtToken();

            LoginRequest loginRequest = AuthRequestsTestHelper.LoginRequest();
            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

            _ = response.EnsureSuccessStatusCode();

            ReadTokenDto? tokenDto = await response.Content.ReadFromJsonAsync<ReadTokenDto>();

            _ = tokenDto.ShouldNotBeNull();
            tokenDto.AccessToken.ShouldNotBeNullOrEmpty();
        }
    }
}
