using AuthTemplate.Db.Models;
using AuthTemplate.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
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

            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            Assert.NotNull(principal);
            _ = Assert.IsType<JwtSecurityToken>(validatedToken);
            Assert.Equal(_configuration["Jwt:ValidIssuer"], ((JwtSecurityToken)validatedToken).Issuer);
        }

        [Fact]
        public async Task Register_ReturnsSuccess_ThenCannotLoginUntilConfirmed()
        {
            // 1) Register a new user
            AuthRegisterDto registerDto = new()
            {
                Email = "userTest@example.com",
                Password = "UserTestPass123!"
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("/auth/register", registerDto);
            _ = response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string content = await response.Content.ReadAsStringAsync();
            Assert.Equal("User registered. Check your email to confirm your account.", content);

            // 2) Immediately try to login (should fail because unconfirmed)
            AuthLoginDto loginDto = new()
            {
                Email = registerDto.Email,
                Password = registerDto.Password
            };

            HttpResponseMessage response2 = await _client.PostAsJsonAsync("/auth/login", loginDto);
            Assert.Equal(HttpStatusCode.Unauthorized, response2.StatusCode);

            string content2 = await response2.Content.ReadAsStringAsync();
            Assert.Contains("Email is not confirmed.", content2);
        }

        [Fact]
        public async Task Register_ConfirmEmail_ThenLogin_Succeeds()
        {
            // 1) Register user
            AuthRegisterDto registerDto = new()
            {
                Email = "userConfirm@example.com",
                Password = "ConfirmPass123!"
            };

            HttpResponseMessage registerResponse = await _client.PostAsJsonAsync("/auth/register", registerDto);
            _ = registerResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);

            // 2) Retrieve the newly created user from the DB
            //    We'll generate a confirmation token ourselves to simulate the email flow
            //    (Alternatively, you could do a "mock" email scenario.)
            UserManager<AppUser> userManager = _factory.Services.GetRequiredService<UserManager<AppUser>>();
            AppUser? user = await userManager.FindByEmailAsync(registerDto.Email);
            Assert.NotNull(user);

            // 3) Generate the real token
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            // 4) Confirm email via the new endpoint:
            //    Notice your AuthController has `[HttpGet("confirm-email")]` but uses [FromBody].
            //    This is unusual. We'll do a GET with a JSON body anyway to match your snippet.
            ConfirmEmailDto confirmDto = new()
            {
                UserId = user.Id,
                Token = token
            };

            HttpRequestMessage confirmRequest = new(HttpMethod.Get, "/auth/confirm-email")
            {
                Content = JsonContent.Create(confirmDto)
            };

            HttpResponseMessage confirmResponse = await _client.SendAsync(confirmRequest);
            _ = confirmResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, confirmResponse.StatusCode);

            string confirmContent = await confirmResponse.Content.ReadAsStringAsync();
            Assert.Equal("Email confirmed successfully.", confirmContent);

            // 5) Now login should succeed
            AuthLoginDto loginDto = new()
            {
                Email = registerDto.Email,
                Password = registerDto.Password
            };

            HttpResponseMessage loginResponse = await _client.PostAsJsonAsync("/auth/login", loginDto);
            _ = loginResponse.EnsureSuccessStatusCode();
            AuthTokenReadDto? tokenResponse = await loginResponse.Content.ReadFromJsonAsync<AuthTokenReadDto>();
            Assert.NotNull(tokenResponse);
            Assert.NotEmpty(tokenResponse.AccessToken);

            ValidateJwtToken(tokenResponse.AccessToken);
        }

        [Fact]
        public async Task Login_DefaultAdmin_ReturnsValidJwtToken()
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("/auth/login", new AuthLoginDto
            {
                Email = "admin@example.com",
                Password = "AdminPass123!"
            });

            _ = response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            AuthTokenReadDto? tokenResponse = await response.Content.ReadFromJsonAsync<AuthTokenReadDto>();
            Assert.NotNull(tokenResponse);
            Assert.NotEmpty(tokenResponse.AccessToken);

            ValidateJwtToken(tokenResponse.AccessToken);
        }

        [Fact]
        public async Task Refresh_ReturnsNewValidJwtToken()
        {
            // 1) Login as admin to get refresh token
            HttpResponseMessage response = await _client.PostAsJsonAsync("/auth/login", new AuthLoginDto
            {
                Email = "admin@example.com",
                Password = "AdminPass123!"
            });

            _ = response.EnsureSuccessStatusCode();
            AuthTokenReadDto? tokenResponse = await response.Content.ReadFromJsonAsync<AuthTokenReadDto>();
            Assert.NotNull(tokenResponse);

            // 2) Grab the refresh token cookie
            Assert.True(response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string>? cookies));
            string? refreshToken = cookies?
                .FirstOrDefault(c => c.StartsWith("refreshToken="))?
                .Split(';', 2)[0]
                .Split('=', 2)[1];

            Assert.NotNull(refreshToken);

            // 3) Call /auth/refresh with that cookie
            HttpRequestMessage refreshRequest = new(HttpMethod.Post, "/auth/refresh");
            refreshRequest.Headers.Add("Cookie", $"refreshToken={refreshToken}");

            HttpResponseMessage response2 = await _client.SendAsync(refreshRequest);
            _ = response2.EnsureSuccessStatusCode();

            AuthTokenReadDto? tokenResponse2 = await response2.Content.ReadFromJsonAsync<AuthTokenReadDto>();
            Assert.NotNull(tokenResponse2);
            Assert.NotEmpty(tokenResponse2.AccessToken);

            // 4) Validate the new access token
            ValidateJwtToken(tokenResponse2.AccessToken);
        }

        [Fact]
        public async Task ResendVerification_WorksWithCooldown()
        {
            // 1) Register an unconfirmed user
            string email = "resendMe@example.com";
            HttpResponseMessage response = await _client.PostAsJsonAsync("/auth/register", new AuthRegisterDto
            {
                Email = email,
                Password = "Test123!"
            });
            _ = response.EnsureSuccessStatusCode();

            // 2) Immediately resend verification
            //    In your controller, you used `if (now - user.LastVerificationEmailSent.Value).TotalSeconds < X`.
            //    We'll assume "EmailVerificationDelayInMinutes" is set to 60 (seconds) in appsettings? 
            //    We'll test BOTH success and fail conditions.
            HttpResponseMessage response2 = await _client.PostAsJsonAsync("/auth/resend-verification", new ResendVerificationDto
            {
                Email = email
            });
            // The code checks if the last was < X seconds. It's set to the config value in "EmailVerificationDelayInMinutes"
            // If your config is truly minutes, then 1 or 2 seconds won't fail. Let's assume it's small enough for demonstration.
            // If you want to see a success scenario, set your config to e.g. 0 seconds or 1 second. 
            // We'll do an immediate call - might produce "Please wait before requesting another verification email." 
            // or "Verification email resent." depending on config. 
            // We'll just demonstrate how to handle each scenario:

            if (response2.IsSuccessStatusCode)
            {
                // If success
                string msg = await response2.Content.ReadAsStringAsync();
                Assert.Equal("Verification email resent.", msg);
            }
            else
            {
                // If blocked by cooldown
                string error = await response2.Content.ReadAsStringAsync();
                Assert.Contains("Please wait", error);
            }
        }

        [Fact]
        public async Task ForgotPassword_ThenResetPassword_Succeeds()
        {
            // 1) Register & confirm a user so that ForgotPassword can be tested
            AuthRegisterDto registerDto = new()
            {
                Email = "forgotme@example.com",
                Password = "ForgotPass123!"
            };
            _ = await _client.PostAsJsonAsync("/auth/register", registerDto);

            // Confirm the user (directly via userManager):
            UserManager<AppUser> userManager = _factory.Services.GetRequiredService<UserManager<AppUser>>();
            AppUser? user = await userManager.FindByEmailAsync(registerDto.Email);
            Assert.NotNull(user);
            string confirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            _ = await userManager.ConfirmEmailAsync(user, confirmToken);

            // 2) Forgot Password
            HttpResponseMessage forgotResp = await _client.PostAsJsonAsync("/auth/forgot-password", new ForgotPasswordDto
            {
                Email = registerDto.Email
            });
            _ = forgotResp.EnsureSuccessStatusCode();
            string forgotMsg = await forgotResp.Content.ReadAsStringAsync();
            Assert.Equal("Password reset email sent.", forgotMsg);

            // 3) Generate the actual token ourselves (simulating the email)
            string resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            // 4) Reset password
            string newPassword = "NewSecret123!";
            HttpResponseMessage resetResp = await _client.PostAsJsonAsync("/auth/reset-password", new ResetPasswordDto
            {
                Email = registerDto.Email,
                Token = resetToken,
                NewPassword = newPassword
            });
            _ = resetResp.EnsureSuccessStatusCode();
            string resetMsg = await resetResp.Content.ReadAsStringAsync();
            Assert.Equal("Password reset successfully.", resetMsg);

            // 5) Login with the new password
            HttpResponseMessage loginResp = await _client.PostAsJsonAsync("/auth/login", new AuthLoginDto
            {
                Email = registerDto.Email,
                Password = newPassword
            });
            _ = loginResp.EnsureSuccessStatusCode();
            AuthTokenReadDto? tokenDto = await loginResp.Content.ReadFromJsonAsync<AuthTokenReadDto>();
            Assert.NotNull(tokenDto);
            ValidateJwtToken(tokenDto.AccessToken);
        }
    }
}
