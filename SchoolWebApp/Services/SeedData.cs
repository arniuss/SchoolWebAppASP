using Microsoft.AspNetCore.Identity;
using SchoolWebApp.Models;

namespace SchoolWebApp.Services
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string adminRole = "Admin";
            string adminEmail = "Admin";
            string adminPassword = "Abcd1234.";

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser { UserName = adminEmail, Email = adminEmail };
                await userManager.CreateAsync(adminUser, adminPassword);
            }

            if (!await userManager.IsInRoleAsync(adminUser, adminRole))
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
    }
}
