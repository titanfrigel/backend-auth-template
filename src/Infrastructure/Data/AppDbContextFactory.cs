using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BackendAuthTemplate.Infrastructure.Data
{
    public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            string cs =
                config.GetConnectionString("AppDb")
                ?? "Host=localhost;Port=5432;Database=_design_time_;Username=postgres;Password=postgres";

            DbContextOptionsBuilder<AppDbContext> builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(cs, npgsql =>
                {
                    _ = npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                });

            return new AppDbContext(builder.Options);
        }
    }
}
