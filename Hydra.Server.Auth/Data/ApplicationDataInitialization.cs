using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Hydra.Server.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Hydra.Server.Auth.Data
{
    using Authorization;

    public static class ApplicationDataInitialization
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            var constants = typeof(GlobalConstants.Role)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            var roles = constants
                .Select(f => f.GetRawConstantValue() as string)
                .ToArray();

            foreach (var role in roles)
            {
                if (await roleManager.RoleExistsAsync(role))
                {
                    continue;
                }

                await roleManager.CreateAsync(new ApplicationRole(role));
            }

            var adminUser = new ApplicationUser()
            {
                Email = configuration["AdminUser"],
                UserName = configuration["AdminUser"],
            };

            if (null == (await userManager.FindByNameAsync(adminUser.UserName)))
            {
                var result = await userManager.CreateAsync(adminUser, configuration["AdminPass"]);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, GlobalConstants.Role.AdministratorRoleName);
                    await userManager.AddClaimAsync(adminUser, new Claim(ClaimTypes.Name, GlobalConstants.Role.AdministratorRoleName));
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(adminUser);
                    await userManager.ConfirmEmailAsync(adminUser, token);
                }
            }

        }
    }
}