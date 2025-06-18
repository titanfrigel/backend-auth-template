namespace BackendAuthTemplate.Application.Common.Settings
{
    public class SecuritySettings
    {
        public required int EmailVerificationDelayInMinutes { get; init; }
        public required int ResetPasswordDelayInMinutes { get; init; }
        public required int UnconfirmedUserCleanupTimeInDays { get; init; }
        public required int UnconfirmedUserCleanupIntervalInDays { get; init; }
    }
}
