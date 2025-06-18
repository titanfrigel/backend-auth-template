namespace BackendAuthTemplate.Application.Common.Settings
{
    public class AuthSettings
    {
        public required string SigningKey { get; init; }
        public required string ValidIssuer { get; init; }
        public required string ValidAudience { get; init; }
        public required int ExpiresInMinutes { get; init; }
        public required int DefaultRefreshExpiresInDays { get; init; }
        public required int RememberMeRefreshExpiresInDays { get; init; }
    }
}
