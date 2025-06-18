using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace BackendAuthTemplate.Infrastructure.Data
{
    public static class InitialiserExtensions
    {
        private static async Task SeedAsync(IServiceScope scope, AppDbContextInitialiser initialiser)
        {
            UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            IUser userContext = scope.ServiceProvider.GetRequiredService<IUser>();

            string adminEmail = "admin@example.com";
            AppUser? adminUser = await userManager.FindByEmailAsync(adminEmail) ?? throw new Exception("Admin user not found!");

            using (userContext.BeginScope(adminUser.Id))
            {
                await initialiser.SeedAsync();
            }
        }

        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();

            AppDbContextInitialiser initialiser = scope.ServiceProvider.GetRequiredService<AppDbContextInitialiser>();

            await initialiser.InitialiseAsync();

            await initialiser.SeedUsersAsync();

            await SeedAsync(scope, initialiser);
        }

        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();

            AppDbContextInitialiser initialiser = scope.ServiceProvider.GetRequiredService<AppDbContextInitialiser>();

            await initialiser.SeedUsersAsync();

            await SeedAsync(scope, initialiser);
        }
    }

    public class AppDbContextInitialiser(ILogger<AppDbContextInitialiser> logger, AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        public async Task InitialiseAsync()
        {
            try
            {
                _ = await context.Database.EnsureDeletedAsync();
                _ = await context.Database.EnsureCreatedAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedUsersAsync()
        {
            try
            {
                await TrySeedUsersAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database with users.");
                throw;
            }
        }


        public async Task TrySeedUsersAsync()
        {
            string[] roleNames = ["Admin", "User"];

            foreach (string roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    _ = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }

            string adminEmail = "admin@example.com";
            AppUser? adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Tom",
                    LastName = "Etnana",
                    PhoneNumber = "+33666666666",
                    Address = "12 Rue de la Rue",
                    City = "Nancy",
                    ZipCode = "54000",
                    CountryCode = "FRA"
                };

                IdentityResult result = await userManager.CreateAsync(adminUser, "AdminPass123!");

                if (result.Succeeded)
                {
                    _ = await userManager.AddToRoleAsync(adminUser, "User");
                    _ = await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            if (!adminUser.EmailConfirmed)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(adminUser);
                _ = await userManager.ConfirmEmailAsync(adminUser, token);
            }

            string normalEmail = "user@example.com";
            AppUser? normalUser = await userManager.FindByEmailAsync(normalEmail);

            if (normalUser == null)
            {
                normalUser = new AppUser
                {
                    UserName = normalEmail,
                    Email = normalEmail,
                    FirstName = "Nana",
                    LastName = "Ettom",
                    PhoneNumber = "+33666666666",
                    Address = "12 Rue de la Rue",
                    City = "Nancy",
                    ZipCode = "54000",
                    CountryCode = "FRA"
                };

                IdentityResult result = await userManager.CreateAsync(normalUser, "UserPass123!");

                if (result.Succeeded)
                {
                    _ = await userManager.AddToRoleAsync(normalUser, "User");
                }
            }

            if (!normalUser.EmailConfirmed)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(normalUser);
                _ = await userManager.ConfirmEmailAsync(normalUser, token);
            }

            _ = await context.SaveChangesAsync();
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
        public async Task TrySeedAsync()
        {
            Category? category = await context.Categories.FirstOrDefaultAsync();
            if (category == null)
            {
                category = new() { Name = "Category 1", Description = "Description 1" };
                _ = context.Categories.Add(category);
            }

            Subcategory? subcategory = await context.Subcategories.FirstOrDefaultAsync();
            if (subcategory == null)
            {
                subcategory = new() { Name = "Subcategory 1", Description = "Description 1", CategoryId = category.Id };
                _ = context.Subcategories.Add(subcategory);
            }

            _ = await context.SaveChangesAsync();
        }
    }
}