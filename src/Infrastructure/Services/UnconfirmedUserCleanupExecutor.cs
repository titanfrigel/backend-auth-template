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
    public class UnconfirmedUserCleanupExecutor(IUser userContext, UserManager<AppUser> userManager, IOptions<SecuritySettings> securityOptions, TimeProvider timeProvider, ILogger<IUnconfirmedUserCleanupExecutor> logger) : IUnconfirmedUserCleanupExecutor
    {
        private readonly SecuritySettings securitySettings = securityOptions.Value;

        public async Task CleanupUnconfirmedAccounts(CancellationToken cancellationToken = default)
        {
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
