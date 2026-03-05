using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BackendAuthTemplate.Infrastructure.Services
{
    public class UnconfirmedUserCleanupWorker(IServiceScopeFactory scopeFactory, IOptions<SecuritySettings> securityOptions, ILogger<IUnconfirmedUserCleanupExecutor> logger) : BackgroundService
    {
        private readonly SecuritySettings securitySettings = securityOptions.Value;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using IServiceScope scope = scopeFactory.CreateScope();
                    IUnconfirmedUserCleanupExecutor executor = scope.ServiceProvider.GetRequiredService<IUnconfirmedUserCleanupExecutor>();

                    await executor.CleanupUnconfirmedAccounts(cancellationToken);

                    await Task.Delay(TimeSpan.FromSeconds(securitySettings.UnconfirmedUserCleanupIntervalInDays), cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while auto-deleting unconfirmed users");
                }
            }
        }
    }

}