using BackendAuthTemplate.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BackendAuthTemplate.Infrastructure.Identity
{
    public class AppUser : IdentityUser<Guid>, IAppUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string ZipCode { get; set; }
        public required string CountryCode { get; set; }
        public string? RefreshTokenHash { get; set; }
        public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
        public DateTimeOffset? LastVerificationEmailSent { get; set; }
        public DateTimeOffset? LastPasswordResetEmailSent { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
