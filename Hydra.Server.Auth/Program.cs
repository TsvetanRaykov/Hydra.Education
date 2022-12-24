using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Server.Auth
{
    using Data;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Storage;
    using Models;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

                    // Initialize the database only if it does not exist
                    if (!dbContext.Database.GetService<IRelationalDatabaseCreator>().Exists())
                    {
                        dbContext.Database.Migrate();
                        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        ApplicationDataInitialization.SeedAsync(userManager, roleManager, configuration).GetAwaiter().GetResult();
                    }

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
