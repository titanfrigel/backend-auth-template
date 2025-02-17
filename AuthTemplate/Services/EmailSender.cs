using Microsoft.AspNetCore.Identity.UI.Services;

namespace AuthTemplate.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
#warning TODO: Implement email sending logic
            return Task.CompletedTask;
        }
    }
}
