using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Settings;
using BackendAuthTemplate.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BackendAuthTemplate.Infrastructure.Services
{
    public class UnconfirmedUserCleanupService(IServiceProvider serviceProvider, IOptions<SecuritySettings> securityOptions, TimeProvider timeProvider, ILogger<UnconfirmedUserCleanupService> logger) : BackgroundService
    {
        private readonly SecuritySettings securitySettings = securityOptions.Value;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupUnconfirmedAccounts(cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while auto-deleting unconfirmed users");
                }

                await Task.Delay(TimeSpan.FromDays(securitySettings.UnconfirmedUserCleanupIntervalInDays), cancellationToken);
            }
        }

        private async Task CleanupUnconfirmedAccounts(CancellationToken cancellationToken = default)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            IUser userContext = scope.ServiceProvider.GetRequiredService<IUser>();

            DateTimeOffset threshold = timeProvider.GetUtcNow().Subtract(TimeSpan.FromDays(securitySettings.UnconfirmedUserCleanupTimeInDays));

            List<AppUser> unconfirmedUsers = await userManager.Users
                .Where(u => !u.EmailConfirmed && u.CreatedAt < threshold)
                .ToListAsync(cancellationToken);

            foreach (AppUser? user in unconfirmedUsers)
            {
                using (userContext.BeginScope(Guid.Empty))
                {
                    _ = await userManager.DeleteAsync(user);

                    logger.LogInformation("Unconfirmed user {UserId} deleted", user.Id);
                }
            }
        }
    }
}
