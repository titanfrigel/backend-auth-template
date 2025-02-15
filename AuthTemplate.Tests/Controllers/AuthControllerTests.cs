using AuthTemplate.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace AuthTemplate.Tests.Controllers
{
    public class AuthControllerTests : IAsyncLifetime
    {
        private NoAuthWebApplicationFactory<Program> _factory = null!;
        private HttpClient _client = null!;
        private IConfiguration _configuration = null!;
        private readonly string _databaseName = $"InMemoryDb_{Guid.NewGuid()}";

        public Task InitializeAsync()
        {
            _factory = new NoAuthWebApplicationFactory<Program>(_databaseName);
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true
            });
            _configuration = _factory.Configuration;

            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            _client.Dispose();
            _factory.Dispose();
            return Task.CompletedTask;
        }
        private void ValidateJwtToken(string token)
        {
            System.Diagnostics.Debug.WriteLine("AT3S");
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]!);

            TokenValidationParameters validationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:ValidAudience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            System.Security.Claims.ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            Assert.NotNull(principal);
            _ = Assert.IsType<JwtSecurityToken>(validatedToken);
            Assert.Equal(_configuration["Jwt:ValidIssuer"], ((JwtSecurityToken)validatedToken).Issuer);
        }

        [Fact]
        public async Task Login_WithHttpRoute_ReturnsValidJwtToken()
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("/auth/login", new AuthLoginDto { Email = "admin@example.com", Password = "AdminPass123!" });

            _ = response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            AuthTokenReadDto? tokenResponse = await response.Content.ReadFromJsonAsync<AuthTokenReadDto>();
            Assert.NotNull(tokenResponse);
            Assert.NotEmpty(tokenResponse.AccessToken);

            ValidateJwtToken(tokenResponse.AccessToken);
        }

        [Fact]
        public async Task Refresh_WithHttpRoute_ReturnsValidJwtToken()
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("/auth/login", new AuthLoginDto { Email = "admin@example.com", Password = "AdminPass123!" });

            _ = response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            AuthTokenReadDto? tokenResponse = await response.Content.ReadFromJsonAsync<AuthTokenReadDto>();
            Assert.NotNull(tokenResponse);

            IEnumerable<string> cookies = response.Headers.GetValues("Set-Cookie");
            string? refreshToken = null;

            foreach (string cookie in cookies)
            {
                if (cookie.StartsWith("refreshToken="))
                {
                    refreshToken = cookie.Split(";")[0].Split("=")[1];
                }
            }

            Assert.NotNull(refreshToken);

            HttpRequestMessage refreshRequest = new(HttpMethod.Post, "/auth/refresh");
            refreshRequest.Headers.Add("Cookie", $"refreshToken={refreshToken}");

            HttpResponseMessage response2 = await _client.SendAsync(refreshRequest);

            _ = response2.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

            AuthTokenReadDto? tokenResponse2 = await response2.Content.ReadFromJsonAsync<AuthTokenReadDto>();
            Assert.NotNull(tokenResponse2);
            Assert.NotEmpty(tokenResponse2.AccessToken);

            ValidateJwtToken(tokenResponse2.AccessToken);
        }

        [Fact]
        public async Task Register_WithHttpRoute_ReturnsSuccess()
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("/auth/register", new AuthLoginDto { Email = "user@example.com", Password = "UserPass123!" });

            _ = response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("User registered successfully", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task RegisterThenLogin_WithHttpRoute_ReturnsValidJwtToken()
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("/auth/register", new AuthLoginDto { Email = "user@example.com", Password = "UserPass123!" });

            _ = response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("User registered successfully", await response.Content.ReadAsStringAsync());

            HttpResponseMessage response2 = await _client.PostAsJsonAsync("/auth/login", new AuthLoginDto { Email = "user@example.com", Password = "UserPass123!" });

            _ = response2.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

            AuthTokenReadDto? tokenResponse = await response2.Content.ReadFromJsonAsync<AuthTokenReadDto>();
            Assert.NotNull(tokenResponse);
            Assert.NotEmpty(tokenResponse.AccessToken);

            ValidateJwtToken(tokenResponse.AccessToken);
        }
    }
}
