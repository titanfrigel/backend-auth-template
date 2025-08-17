namespace BackendAuthTemplate.Domain.Interfaces
{
    public interface IAppUser
    {
        Guid Id { get; set; }
        string? Email { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string? PhoneNumber { get; set; }
        string Address { get; set; }
        string City { get; set; }
        string ZipCode { get; set; }
        string CountryCode { get; set; }
        string? RefreshTokenHash { get; set; }
        DateTimeOffset? RefreshTokenExpiryTime { get; set; }
        string? PreviousRefreshTokenHash { get; set; }
        DateTimeOffset? PreviousRefreshTokenValidUntil { get; set; }
        DateTimeOffset? LastVerificationEmailSent { get; set; }
        DateTimeOffset? LastPasswordResetEmailSent { get; set; }
        DateTimeOffset CreatedAt { get; set; }
    }
}
