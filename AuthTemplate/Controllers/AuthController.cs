using AuthTemplate.Db.Models;
using AuthTemplate.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthTemplate.Controllers
{
    [ApiController]
    [Route("auth")]
    [AllowAnonymous]
    public class AuthController(IConfiguration configuration, UserManager<AppUser> userManager, IEmailSender emailSender) : ControllerBase
    {
        #region HELLPER FUNCTIONS
        private string GetEmailConfirmationHtml(string confirmationLink)
        {
            return $@"
                <html>
                    <body>
                        <h1>Confirm your email</h1>
                        <p>Please confirm your account by clicking <a href='{confirmationLink}'>here</a>.</p>
                    </body>
                </html>
            ";
        }

        private string GetResetPasswordHtml(string confirmationLink)
        {
            return $@"
                <html>
                    <body>
                        <h1>Confirm your email</h1>
                        <p>Please confirm your account by clicking <a href='{confirmationLink}'>here</a>.</p>
                    </body>
                </html>
            ";
        }

        private string GetEmailConfirmationLink(AppUser user, string token)
        {
            return $"{configuration["FrontendUri"]}/verify-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";
        }

        private string GetResetPasswordLink(AppUser user, string token)
        {
            return $"{configuration["FrontendUri"]}/reset-password?userId={user.Id}&token={Uri.EscapeDataString(token)}";
        }

        private string GenerateJwtToken(AppUser user)
        {
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(configuration["Jwt:SigningKey"]!));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = [
                new Claim("sub", user.Id),
                new Claim("email", user.Email!),
            ];

            IList<string> roles = userManager.GetRolesAsync(user).Result;
            foreach (string role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            JwtSecurityToken token = new(
                issuer: configuration["Jwt:ValidIssuer"],
                audience: configuration["Jwt:ValidAudience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpiresInMinutes")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private void SetTokenCookies(string refreshToken, DateTime refreshTokenExpiryDate)
        {
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = refreshTokenExpiryDate
            });
        }

        #endregion

        #region REGISTER

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterDto registerDto)
        {
            if (userManager.FindByEmailAsync(registerDto.Email).Result != null)
            {
                return BadRequest("Email already in use");
            }

            AppUser user = new()
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };

            IdentityResult result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            _ = await userManager.AddToRoleAsync(user, "User");

            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            await emailSender.SendEmailAsync(
                user.Email,
                "Confirm your email",
                GetEmailConfirmationHtml(GetEmailConfirmationLink(user, token))
            );

            user.LastVerificationEmailSent = DateTime.UtcNow;
            _ = await userManager.UpdateAsync(user);

            return Ok("User registered. Check your email to confirm your account.");
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmailDto)
        {
            AppUser? user = await userManager.FindByIdAsync(confirmEmailDto.UserId);
            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            IdentityResult result = await userManager.ConfirmEmailAsync(user, confirmEmailDto.Token);

            return !result.Succeeded
                ? BadRequest("Email confirmation failed.")
                : Ok("Email confirmed successfully.");
        }

        #endregion

        #region RESEND VERIFICATION

        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationDto resendVerificationDto)
        {
            AppUser? user = await userManager.FindByEmailAsync(resendVerificationDto.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (await userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest("Email already confirmed.");
            }

            DateTime now = DateTime.UtcNow;
            if (user.LastVerificationEmailSent.HasValue &&
                (now - user.LastVerificationEmailSent.Value).TotalSeconds < configuration.GetValue<int>("EmailVerificationDelayInMinutes"))
            {
                return BadRequest("Please wait before requesting another verification email.");
            }

            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            await emailSender.SendEmailAsync(
                user.Email!,
                "Confirm your email",
                GetEmailConfirmationHtml(GetEmailConfirmationLink(user, token))
            );

            user.LastVerificationEmailSent = now;
            _ = await userManager.UpdateAsync(user);

            return Ok("Verification email resent.");
        }

        #endregion

        #region FORGOT & RESET PASSWORD

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            AppUser? user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            DateTime now = DateTime.UtcNow;
            if (user.LastPasswordResetEmailSent.HasValue
                && (now - user.LastPasswordResetEmailSent.Value).TotalMinutes < configuration.GetValue<int>("ResetPasswordDelayInMinutes"))
            {
                return BadRequest("Please wait a few minutes before requesting another password reset email.");
            }

            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            _ = $"{configuration["FrontendUrl"]}/reset-password?email={Uri.EscapeDataString(user.Email!)}&token={Uri.EscapeDataString(token)}";

            await emailSender.SendEmailAsync(
                user.Email!,
                "Reset Password",
                GetResetPasswordHtml(GetResetPasswordLink(user, token))
            );

            user.LastPasswordResetEmailSent = now;
            _ = await userManager.UpdateAsync(user);

            return Ok("Password reset email sent.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            AppUser? user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            IdentityResult result = await userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

            return !result.Succeeded ? BadRequest(result.Errors) : Ok("Password reset successfully.");
        }

        #endregion

        #region LOGIN & REFRESH TOKEN

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginDto loginDto)
        {
            AppUser? user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized("Invalid credentials.");
            }

            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                return Unauthorized("Email is not confirmed.");
            }

            string token = GenerateJwtToken(user);
            string refreshToken = GenerateRefreshToken();

            int refreshExpiresInDays = loginDto.RememberMe
               ? configuration.GetValue<int>("Jwt:RememberMeRefreshExpiresInDays")
               : configuration.GetValue<int>("Jwt:DefaultRefreshExpiresInDays");

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshExpiresInDays);

            _ = await userManager.UpdateAsync(user);

            SetTokenCookies(refreshToken, user.RefreshTokenExpiryTime!.Value);

            return Ok(new AuthTokenReadDto { AccessToken = token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out string? refreshToken) || refreshToken == null)
            {
                return Unauthorized("No refresh token found.");
            }

            AppUser? user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid refresh token.");
            }

            string newToken = GenerateJwtToken(user);
            string newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            _ = await userManager.UpdateAsync(user);

            SetTokenCookies(newRefreshToken, user.RefreshTokenExpiryTime!.Value);

            return Ok(new AuthTokenReadDto { AccessToken = newToken });
        }

        #endregion
    }
}
