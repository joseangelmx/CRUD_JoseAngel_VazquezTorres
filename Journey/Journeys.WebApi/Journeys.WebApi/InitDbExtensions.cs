using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Journeys.WebApi
{
    public static class InitDbExtensions
    {
        public static IApplicationBuilder InitDb(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetService<UserManager<IdentityUser>>();
                var roleManager = services.GetService<RoleManager<IdentityRole>>();
                if (userManager.Users.Count() == 0)
                {
                    Task.Run(() => InitRoles(roleManager)).Wait();
                    Task.Run(() => InitUsers(userManager)).Wait();
                }
            }
            return app;
        }
        private static async Task InitRoles(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                var role = new IdentityRole("Admin");
                await roleManager.CreateAsync(role);

                role = new IdentityRole("User");
                await roleManager.CreateAsync(role);

                role = new IdentityRole("Supervisor");
                await roleManager.CreateAsync(role);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private static async Task InitUsers(UserManager<IdentityUser> userManager)
        {
            var user = new IdentityUser()
            {
                UserName = "admin@website.com",
                Email = "admin@website.com",
                PhoneNumber = "12345678"
            };
            await userManager.CreateAsync(user, "3eJ0eMN@*wl9+Q5uT#x42c8P$K7%yN1b");
            await userManager.AddToRoleAsync(user, "Admin");
        }
        public static Int64 ToUnixEpochDate(this DateTime dateTime)
        {
            var unixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var timeSinceEpoch = dateTime.ToUniversalTime() - unixEpoch;
            var unixTimeSeconds = (Int64)Math.Round(timeSinceEpoch.TotalSeconds);
            return unixTimeSeconds;
        }
    }
}
