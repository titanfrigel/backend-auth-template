using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Domain.Interfaces;
using BackendAuthTemplate.EmailComponents;
using BackendAuthTemplate.Infrastructure.Interfaces;

namespace BackendAuthTemplate.Infrastructure.Services
{
    public class EmailService(IHtmlRenderer renderer, IEmailSender sender) : IEmailService
    {
        public async Task SendConfirmEmailAsync(IAppUser user, string confirmLink, CancellationToken cancellationToken = default)
        {
            string html = await renderer.RenderAsync<ConfirmEmailTemplate>(new Dictionary<string, object?>()
            {
                ["FirstName"] = user.FirstName,
                ["ConfirmUrl"] = confirmLink,
                ["ValidHours"] = 4
            }, cancellationToken);

            await sender.SendEmailAsync(EmailAccount.Noreply, user.Email!, user.FirstName, "Confirmation d'email", html, cancellationToken: cancellationToken);
        }

        public async Task SendResetPasswordEmailAsync(IAppUser user, string resetLink, CancellationToken cancellationToken = default)
        {
            string html = await renderer.RenderAsync<ResetPasswordTemplate>(new Dictionary<string, object?>()
            {
                ["ResetUrl"] = resetLink,
                ["ValidMinutes"] = 30
            }, cancellationToken);

            await sender.SendEmailAsync(EmailAccount.Noreply, user.Email!, user.FirstName, "Réinitialisation du mot de passe", html, cancellationToken: cancellationToken);
        }
    }
}
