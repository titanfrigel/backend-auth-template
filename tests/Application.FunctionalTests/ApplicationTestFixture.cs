using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Settings;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Infrastructure.Data.Interceptors;
using BackendAuthTemplate.Infrastructure.Identity;
using BackendAuthTemplate.Infrastructure.Interfaces;
using BackendAuthTemplate.Infrastructure.Services;
using BackendAuthTemplate.Tests.Common.Moqs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackendAuthTemplate.Application.FunctionalTests
{
    public class ApplicationTestFixture
    {
        public ServiceProvider ServiceProvider { get; }
        private readonly string _dbName = "TestDb_" + Guid.NewGuid().ToString();

        public ApplicationTestFixture()
        {
            ServiceCollection services = new();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile("appsettings.Testing.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            _ = services.AddSingleton<IConfiguration>(configuration);

            _ = services.Configure<AuthSettings>(configuration.GetSection("Auth"));
            _ = services.Configure<EmailSettings>(configuration.GetSection("Emails"));
            _ = services.Configure<GeneralSettings>(configuration.GetSection("General"));
            _ = services.Configure<SecuritySettings>(configuration.GetSection("Security"));
            _ = services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));

            _ = services.AddApplicationServices();

            _ = services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            _ = services.AddScoped<ISaveChangesInterceptor, SoftDeleteInterceptor>();
            _ = services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            _ = services.AddDbContext<AppDbContext>((sp, options) =>
            {
                _ = options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                _ = options.UseSqlite($"DataSource=file:memdb{_dbName}?mode=memory&cache=shared");
            });

            _ = services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

            _ = services.AddScoped<AppDbContextInitialiser>();

            _ = services.AddSingleton(TimeProvider.System);
            _ = services.AddDataProtection();

            _ = services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;

                options.Tokens.ProviderMap.Add("CustomEmailConfirmation", new TokenProviderDescriptor(typeof(CustomEmailConfirmationTokenProvider<AppUser>)));
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                options.Tokens.ProviderMap.Add("CustomPasswordReset", new TokenProviderDescriptor(typeof(CustomPasswordResetTokenProvider<AppUser>)));
                options.Tokens.PasswordResetTokenProvider = "CustomPasswordReset";
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

            _ = services.AddTransient<CustomEmailConfirmationTokenProvider<AppUser>>();
            _ = services.AddTransient<CustomPasswordResetTokenProvider<AppUser>>();

            _ = services.AddSingleton<IHtmlRenderer, RazorHtmlRenderer>();

            _ = services.AddScoped<IEmailService, EmailService>();
            _ = services.AddScoped<ITokenService, TokenService>();
            _ = services.AddScoped<IIdentityService, IdentityService>();

            _ = services.AddScoped<IEmailSender, FakeEmailSender>();
            _ = services.AddScoped<ICookieService, FakeCookieService>();
            _ = services.AddScoped<IUser, FakeCurrentUser>();

            ServiceProvider = services.BuildServiceProvider();

            AppDbContextInitialiser initialiser = ServiceProvider.GetRequiredService<AppDbContextInitialiser>();

            UserManager<AppUser> userManager = ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            IUser userContext = ServiceProvider.GetRequiredService<IUser>();

            initialiser.InitialiseAsync().Wait();
            initialiser.SeedUsersAsync().Wait();

            string adminEmail = "admin@example.com";
            AppUser? adminUser = userManager.FindByEmailAsync(adminEmail).Result ?? throw new Exception("Admin user not found!");

            using (userContext.BeginScope(adminUser.Id))
            {
                initialiser.SeedAsync().Wait();
            }
        }
    }
}
