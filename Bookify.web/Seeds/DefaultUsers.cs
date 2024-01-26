﻿using Microsoft.AspNetCore.Identity;

namespace Bookify.web.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            var admin = new ApplicationUser
            {
                FullName = "Admin",
                EmailConfirmed = true,
                UserName = "admin",
                Email = "Admin@Bookify.com"
            };
            
            var user = await userManager.FindByEmailAsync(admin.Email);
            if(user is null)
            {
                await userManager.CreateAsync(admin, "P@ssword123");
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);

            }
        }
    }
}
