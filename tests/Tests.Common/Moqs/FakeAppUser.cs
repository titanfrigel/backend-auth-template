using BackendAuthTemplate.Domain.Interfaces;

namespace BackendAuthTemplate.Tests.Common.Moqs
{
    public class FakeAppUser() : IAppUser
    {
        public Guid Id { get; set; }
        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string? RefreshTokenHash { get; set; }
        public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
        public DateTimeOffset? LastVerificationEmailSent { get; set; }
        public DateTimeOffset? LastPasswordResetEmailSent { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
