using CarStory.Data;
using CarStory.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarStory.Infrastructure
{
    public  static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<ApplicationDbContext>();

            data.Database.Migrate();

            SeedAdmin(scopedServices.ServiceProvider);
            //Seed(data);

            return app;
        }

        private static void SeedAdmin(IServiceProvider services) 
        {
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync("Admin"))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = "Admin" };

                    await roleManager.CreateAsync(role);

                    const string adminUsername = "admin";
                    const string adminEmail = "admin@admin.com";
                    const string adminPassword = "123456";

                    var user = new AppUser
                    {
                        Email = adminEmail,
                        UserName = adminUsername
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
