using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BackendAuthTemplate.Infrastructure.Services
{
    public class TokenService(IOptions<AuthSettings> jwtOptions, TimeProvider timeProvider) : ITokenService
    {
        private readonly AuthSettings jwtSettings = jwtOptions.Value;
        private const int WorkFactor = 11;

        public string GenerateAccessToken(Guid userId, string userEmail, string firstName, string lastName, IList<string> userRoles)
        {
            SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(jwtSettings.SigningKey));
            SigningCredentials credentials = new(signingKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = [
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, $"{firstName} {lastName}"),
                new Claim(JwtRegisteredClaimNames.Email, userEmail)
            ];

            foreach (string role in userRoles)
            {
                claims.Add(new Claim("role", role));
            }

            DateTimeOffset utcNow = timeProvider.GetUtcNow().UtcDateTime;

            JwtSecurityToken token = new(
                issuer: jwtSettings.ValidIssuer,
                audience: jwtSettings.ValidAudience,
                claims: claims,
                notBefore: utcNow.UtcDateTime,
                expires: utcNow.AddMinutes(Convert.ToDouble(jwtSettings.ExpiresInMinutes)).UtcDateTime,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string GenerateRefreshTokenComposite(Guid userId, string refreshToken)
        {
            return $"{userId}.{refreshToken}";
        }

        public string Hash(string token)
        {
            return BCrypt.Net.BCrypt.HashPassword(token, workFactor: WorkFactor);
        }

        public bool Verify(string token, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(token, hash);
        }
    }
}
