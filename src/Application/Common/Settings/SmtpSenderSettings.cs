namespace BackendAuthTemplate.Application.Common.Settings
{
    public class SmtpSenderSettings
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
        public required string Email { get; init; }
        public required string Name { get; init; }
    }
}
