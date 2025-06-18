namespace BackendAuthTemplate.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid userId, string userEmail, string firstName, string lastName, IList<string> userRoles);
        string GenerateRefreshToken();
        string GenerateRefreshTokenComposite(Guid userId, string refreshToken);
        string Hash(string token);
        bool Verify(string token, string hash);
    }
}
