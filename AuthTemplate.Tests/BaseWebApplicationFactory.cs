using AuthTemplate.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AuthTemplate.Tests
{
    public abstract class BaseWebApplicationFactory<TStartup>(string databaseName) : WebApplicationFactory<TStartup> where TStartup : class
    {
        public IConfiguration Configuration { get; private set; } = null!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            _ = builder.UseEnvironment("Testing");

            _ = builder.ConfigureAppConfiguration((context, config) =>
            {
                Configuration = config.AddJsonFile("appsettings.json")
                                      .AddJsonFile("appsettings.Testing.json")
                                      .AddUserSecrets<TStartup>()
                                      .AddEnvironmentVariables()
                                      .Build();
            });

            _ = builder.ConfigureServices(services =>
            {
                _ = services.AddDbContext<AppDbContext>(options =>
                {
                    _ = options.UseInMemoryDatabase(databaseName);
                });

                ServiceProvider sp = services.BuildServiceProvider();

                using IServiceScope scope = sp.CreateScope();
                AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                _ = dbContext.Database.EnsureCreated();
            });
        }
    }
}



