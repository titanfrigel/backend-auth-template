using BackendAuthTemplate.Infrastructure.Interfaces;

namespace BackendAuthTemplate.Tests.Common.Moqs
{
    public class FakeEmailSender : IEmailSender
    {
        public Task SendEmailAsync(EmailAccount emailAccount, string toEmail, string toName, string subject, string body, string? replyTo = null, string? replyToName = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
