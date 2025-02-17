using AuthTemplate.Db.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthTemplate.Services
{
    public class UnconfirmedUserCleanupService(IServiceProvider serviceProvider, IConfiguration configuration) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanupUnconfirmedAccounts();
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }

        private async Task CleanupUnconfirmedAccounts()
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            DateTime threshold = DateTime.UtcNow.AddDays(-configuration.GetValue<int>("UnconfirmedUserCleanupTimeInDays"));

            List<AppUser> unconfirmedUsers = userManager.Users
                .Where(u => !u.EmailConfirmed && u.CreatedAt < threshold)
                .ToList();

            foreach (AppUser? user in unconfirmedUsers)
            {
                _ = await userManager.DeleteAsync(user);
            }
        }
    }
}
