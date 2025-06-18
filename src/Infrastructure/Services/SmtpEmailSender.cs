using BackendAuthTemplate.Application.Common.Settings;
using BackendAuthTemplate.Infrastructure.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace BackendAuthTemplate.Infrastructure.Services
{
    public class SmtpEmailSender(IOptions<SmtpSettings> smtpOptions, ILogger<EmailService> logger) : IEmailSender
    {
        private readonly SmtpSettings smtpSettings = smtpOptions.Value;

        public async Task SendEmailAsync(EmailAccount emailAccount, string toEmail, string toName, string subject, string body, string? replyTo = null, string? replyToName = null, CancellationToken cancellationToken = default)
        {
            SmtpSenderSettings fromSettings = emailAccount switch
            {
                EmailAccount.Notify => smtpSettings.NotifyFrom,
                EmailAccount.Noreply => smtpSettings.NoreplyFrom,
                _ => throw new NotImplementedException()
            };

            MimeMessage message = new();
            message.From.Add(new MailboxAddress(fromSettings.Name, fromSettings.Email));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;
            if (replyTo != null && replyToName != null)
            {
                message.ReplyTo.Add(new MailboxAddress(replyToName, replyTo));
            }
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using SmtpClient client = new();
            try
            {
                await client.ConnectAsync(smtpSettings.Server, smtpSettings.Port, MailKit.Security.SecureSocketOptions.SslOnConnect, cancellationToken);

                await client.AuthenticateAsync(fromSettings.Username, fromSettings.Password, cancellationToken);

                string response = await client.SendAsync(message, cancellationToken);

                logger.LogInformation("Email sent successfully: {response}", response);

                await client.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while sending an email");
                throw;
            }
        }
    }

}
