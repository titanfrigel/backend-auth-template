namespace BackendAuthTemplate.Application.Common.Settings
{
    public class SmtpSettings
    {
        public required string Server { get; init; }
        public required int Port { get; init; }
        public required SmtpSenderSettings NotifyFrom { get; init; }
        public required SmtpSenderSettings NoreplyFrom { get; init; }
    }
}
