using AuthTemplate.Db.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthTemplate
{
    public static class SeedRolesAndAdmin
    {
        public static async Task SeedRolesAndAdminAsync(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            string[] roleNames = { "Admin", "User" };

            foreach (string roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    _ = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            string adminEmail = "admin@example.com";
            AppUser? adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser { UserName = adminEmail, Email = adminEmail };

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
                normalUser = new AppUser { UserName = normalEmail, Email = normalEmail };

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
        }

    }
}
