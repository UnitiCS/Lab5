using Bakery.Controllers;
using Bakery.Data;
using Bakery.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;


namespace Bakery
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            IServiceCollection services = builder.Services;

            string connection = builder.Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<BakeryDBContext>(options => options.UseSqlServer(connection));

            // Добавление ApplicationDbContext для взаимодействия с базой данных учетных записей
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Добавление сервисов Identity
            services.AddIdentity<IdentityUser, IdentityRole>()
               .AddSignInManager<SignInManager<IdentityUser>>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultUI()
               .AddDefaultTokenProviders();
            
            services.AddTransient<UsersController>();


            await DbUserInitializer.Initialize(services.BuildServiceProvider());

            builder.Services.AddControllersWithViews();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            builder.Services.AddControllersWithViews(options =>
            {
                options.CacheProfiles.Add("BakeryDBCache",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.Any,
                        Duration = 2 * 27 + 240
                    });
            });
            services.AddRazorPages();
            // MVC
            builder.Services.AddControllersWithViews();
            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseDbInitializer();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Использование обработчика маршрутов
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.Run();
        }

        // Роли и пользователи
        //await SeedRolesAndUsersAsync(services.BuildServiceProvider());
        // Метод для создания ролей и пользователей
        /*private static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Создание ролей
                var roles = new[] { "Admin", "User" };
                foreach (var roleName in roles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Создание пользователя с ролью Admin
                var adminEmail = "admin@example.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                    await userManager.CreateAsync(adminUser, "_Admin666");

                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                // Создание пользователя с ролью User
                var userEmail = "user@example.com";
                var user = await userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    user = new IdentityUser { UserName = userEmail, Email = userEmail };
                    await userManager.CreateAsync(user, "_User111");

                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }*/
    }
}
