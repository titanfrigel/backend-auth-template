using BackendAuthTemplate.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BackendAuthTemplate.Infrastructure.Services
{
    public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
    {
        public void SetRefreshToken(string refreshToken, DateTimeOffset refreshTokenExpiryDate)
        {
            CookieOptions options = new()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = refreshTokenExpiryDate
            };

            httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, options);
        }

        public void RemoveRefreshToken()
        {
            httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
        }

        public string? GetRefreshToken()
        {
            return httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
        }
    }
}
