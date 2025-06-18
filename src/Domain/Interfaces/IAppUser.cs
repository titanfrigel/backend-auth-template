namespace BackendAuthTemplate.Domain.Interfaces
{
    public interface IAppUser
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }
        public string? RefreshTokenHash { get; set; }
        public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
        public DateTimeOffset? LastVerificationEmailSent { get; set; }
        public DateTimeOffset? LastPasswordResetEmailSent { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
