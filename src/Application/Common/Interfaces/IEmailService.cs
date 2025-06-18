using BackendAuthTemplate.Domain.Interfaces;

namespace BackendAuthTemplate.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmEmailAsync(IAppUser user, string confirmLink, CancellationToken cancellationToken = default);
        Task SendResetPasswordEmailAsync(IAppUser user, string resetLink, CancellationToken cancellationToken = default);

    }
}
