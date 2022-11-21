using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwigaCRM.Models;

namespace TwigaCRM.Data
{
    public static class SeedUser
    {
        public static async Task Initializer(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            context.Database.EnsureCreated();
            int id = 1;
            const string ADMIN_ID = "b4280b6a-0613-4cbd-a9e6-f1701e926e73";
            const string password = "123456789";

            if (await userManager.FindByEmailAsync("devkim@app.com") == null)
            {

                var user = new AppUser
                {
                    Id = ADMIN_ID,
                    UserName = "superadmin@twigacrm.com",
                    Email = "superadmin@twigacrm.com",
                    EmailConfirmed = true,
                    UserAppRoleId = id,
                    FirstName = "Super",
                    LastName = "Admin",
                    IsActivated = true

                };
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                }
            }

        }
    }
}
