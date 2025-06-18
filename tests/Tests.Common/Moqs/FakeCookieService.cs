using BackendAuthTemplate.Application.Common.Interfaces;

namespace BackendAuthTemplate.Tests.Common.Moqs
{
    public class FakeCookieService : ICookieService
    {
        public void RemoveRefreshToken() { }
        public void SetRefreshToken(string refreshToken, DateTimeOffset refreshTokenExpiryDate) { }
        public string? GetRefreshToken() { return null; }
    }
}
