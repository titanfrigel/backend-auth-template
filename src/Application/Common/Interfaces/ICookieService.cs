namespace BackendAuthTemplate.Application.Common.Interfaces
{
    public interface ICookieService
    {
        void SetRefreshToken(string refreshToken, DateTimeOffset refreshTokenExpiryDate);
        void RemoveRefreshToken();
        string? GetRefreshToken();
    }
}
