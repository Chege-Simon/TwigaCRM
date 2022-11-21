using TwigaCRM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwigaCRM.Data
{
    public static class Seeder
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            int id = 1;
            //int UserId = 1;
            string[] permissions = new string[] { "create_roles", "create_permissions", "view_roles", "view_permissions",
                "assign_permission","disable_permission","view_role_details","import_permissions" };

            const string ADMIN_ID = "b4280b6a-0613-4cbd-a9e6-f1701e926e73";
            const string password = "Super@Admin123";


            modelBuilder.Entity<AppRole>().HasData(
                new AppRole
                {
                    Id = id,
                    Title = "Default Admin",
                    Description = "Default Admin Role"
                }
                );
            modelBuilder.Entity<Region>().HasData(
                new Region
                {
                    Id = id,
                    Name = "Country"
                }
                );
            modelBuilder.Entity<Town>().HasData(
                new Town
                {
                    Id = id,
                    Name = "All",
                    RegionId = 1
                }
                );
            foreach (var permission in permissions)
            {
                modelBuilder.Entity<Permission>().HasData(
                    new Permission
                    {
                        Id = id,
                        Name = permission,
                        Description = "Default Admin permissions"
                    }
                    );
                id++;
            }
            id = 1;
            foreach (var permission in permissions)
            {
                modelBuilder.Entity<AppRole_Permission>().HasData(
                    new AppRole_Permission
                    {
                        Id = id,
                        AppRoleId = 1,
                        PermissionId = id,
                    }
                    );
                id++;
            }
            id = 1;
            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(
                   new AppUser
                   {
                       Id = ADMIN_ID,
                       UserName = "superadmin@twigacrm.com",
                       Email = "superadmin@twigacrm.com",
                       NormalizedUserName = "SUPERADMIN@TWIGACRM.COM",
                       NormalizedEmail = "SUPERADMIN@TWIGACRM.COM",
                       EmailConfirmed = true,
                       UserAppRoleId = id,
                       FirstName = "Super",
                       LastName = "Admin",
                       TownId = 1,
                       PasswordHash = hasher.HashPassword(null, password),

                   }
                );
        }
    }
}
