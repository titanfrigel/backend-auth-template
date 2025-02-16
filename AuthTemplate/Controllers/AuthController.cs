using AuthTemplate.Db.Models;
using AuthTemplate.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class AuthController(IConfiguration configuration, UserManager<AppUser> userManager) : ControllerBase
    {
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

        private void SetTokenCookies(string refreshToken)
        {
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("Jwt:RefreshExpiresInDays"))
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterDto registerDto)
        {
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

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginDto loginDto)
        {
            AppUser? user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized();
            }

            string token = GenerateJwtToken(user);
            string refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(configuration.GetValue<int>("Jwt:RefreshExpiresInDays"));

            _ = await userManager.UpdateAsync(user);

            SetTokenCookies(refreshToken);

            return Ok(new AuthTokenReadDto { AccessToken = token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out string? refreshToken) || refreshToken == null)
            {
                return Unauthorized();
            }

            AppUser? user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized();
            }

            string newToken = GenerateJwtToken(user);
            string newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            _ = await userManager.UpdateAsync(user);

            SetTokenCookies(newRefreshToken);

            return Ok(new AuthTokenReadDto { AccessToken = newToken });
        }
    }
}
