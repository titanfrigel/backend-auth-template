namespace BackendAuthTemplate.Infrastructure.Interfaces
{
    public enum EmailAccount
    {
        Notify,
        Noreply
    }

    public interface IEmailSender
    {
        Task SendEmailAsync(
            EmailAccount emailAccount,
            string toEmail,
            string toName,
            string subject,
            string body,
            string? replyTo = null,
            string? replyToName = null,
            CancellationToken cancellationToken = default
        );
    }

}
