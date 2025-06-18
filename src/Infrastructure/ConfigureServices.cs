using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Settings;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Infrastructure.Data.Interceptors;
using BackendAuthTemplate.Infrastructure.Identity;
using BackendAuthTemplate.Infrastructure.Interfaces;
using BackendAuthTemplate.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BackendAuthTemplate.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _ = services.Configure<AuthSettings>(configuration.GetSection("Auth"));
            _ = services.Configure<EmailSettings>(configuration.GetSection("Emails"));
            _ = services.Configure<GeneralSettings>(configuration.GetSection("General"));
            _ = services.Configure<SecuritySettings>(configuration.GetSection("Security"));
            _ = services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));

            _ = services.AddHttpClient();

            _ = services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            _ = services.AddScoped<ISaveChangesInterceptor, SoftDeleteInterceptor>();
            _ = services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            if (!environment.IsEnvironment("Testing"))
            {
                _ = services.AddDbContext<AppDbContext>((sp, options) =>
                {
                    _ = options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                    _ = options.UseNpgsql(configuration.GetConnectionString("AppDb"));
                });
            }

            _ = services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

            _ = services.AddScoped<AppDbContextInitialiser>();

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

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

            _ = services.AddTransient<CustomEmailConfirmationTokenProvider<AppUser>>();
            _ = services.AddTransient<CustomPasswordResetTokenProvider<AppUser>>();

            _ = services.AddHostedService<UnconfirmedUserCleanupService>();

            _ = services.AddSingleton<IHtmlRenderer, RazorHtmlRenderer>();

            _ = services.AddScoped<IUser, CurrentUser>();
            _ = services.AddScoped<IEmailSender, SmtpEmailSender>();
            _ = services.AddScoped<IEmailService, EmailService>();
            _ = services.AddScoped<ICookieService, CookieService>();
            _ = services.AddScoped<ITokenService, TokenService>();
            _ = services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}
