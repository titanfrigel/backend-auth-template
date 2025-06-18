using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Infrastructure.Identity;
using BackendAuthTemplate.Infrastructure.Interfaces;
using BackendAuthTemplate.Tests.Common.Moqs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BackendAuthTemplate.API.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private IServiceProvider? _serviceProvider;
        private readonly string _dbName = "TestDb_" + Guid.NewGuid().ToString();

        public IServiceProvider ServiceProvider => _serviceProvider!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            _ = builder.UseEnvironment("Testing");

            _ = builder.ConfigureAppConfiguration((context, config) =>
            {
                _ = config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

                _ = config.AddJsonFile("appsettings.Testing.json", optional: true, reloadOnChange: false);

                _ = config.AddEnvironmentVariables();
            });

            _ = builder.ConfigureTestServices(services =>
            {
                _ = services.AddDbContext<AppDbContext>((sp, options) =>
                {
                    _ = options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                    _ = options.UseSqlite($"DataSource=file:memdb{_dbName}?mode=memory&cache=shared");
                });

                _serviceProvider = services.BuildServiceProvider();

                using IServiceScope scope = _serviceProvider.CreateScope();
                AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                _ = dbContext.Database.EnsureCreated();

                _ = services.RemoveAll<IEmailSender>();

                _ = services.AddScoped<IEmailSender, FakeEmailSender>();
            });
        }

        public async Task<Func<IDisposable>> GetScopedUserContextAs(string email)
        {
            using IServiceScope scope = _serviceProvider!.CreateScope();

            IUser userContext = scope.ServiceProvider.GetRequiredService<IUser>();
            UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            AppUser? user = await userManager.FindByEmailAsync(email) ?? throw new Exception("User not found in the test database.");

            return () => userContext.BeginScope(user.Id);
        }

        public async Task<string> GenerateJwtTokenAsync(string email)
        {
            using IServiceScope scope = _serviceProvider!.CreateScope();

            UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            ITokenService tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

            AppUser? user = await userManager.FindByEmailAsync(email) ?? throw new Exception("User not found in the test database.");
            IList<string> roles = await userManager.GetRolesAsync(user);

            return tokenService.GenerateAccessToken(user.Id, user.Email!, user.FirstName, user.LastName, roles);
        }

        public async Task<string> GenerateAdminJwtTokenAsync()
        {
            return await GenerateJwtTokenAsync("admin@example.com");
        }

        public async Task<string> GenerateUserJwtTokenAsync()
        {
            return await GenerateJwtTokenAsync("user@example.com");
        }
    }
}
