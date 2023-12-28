using Bakery.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Bakery
{
    public static class DbUserInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Применение миграций, если база данных еще не существует
                await dbContext.Database.MigrateAsync();

                string superAdminEmail = "superadmin@example.com";
                string adminEmail = "admin@example.com";
                string password = "_Admin666";
                string superpass = "_Admin999";

                // Создание ролей
                var roles = new[] { "Admin", "User", "SuperAdmin" }; // Добавлена новая роль "SuperAdmin"
                foreach (var roleName in roles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Создание пользователя с ролью SuperAdmin
                var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);
                if (superAdminUser == null)
                {
                    superAdminUser = new IdentityUser { UserName = superAdminEmail, Email = superAdminEmail };
                    await userManager.CreateAsync(superAdminUser, superpass);

                    await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                }

                // Создание пользователя с ролью Admin
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                    await userManager.CreateAsync(adminUser, password);

                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                // Создание пользователя с ролью User
                /*var userEmail = "user@example.com";
                var user = await userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    user = new IdentityUser { UserName = userEmail, Email = userEmail };
                    await userManager.CreateAsync(user, "_User111");

                    await userManager.AddToRoleAsync(user, "User");
                }*/
            }
        }
    }
}
