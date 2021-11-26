using System.IO;
using System.Reflection;
using Hydra.Module.Video.Backend.Authentication;
using Hydra.Module.Video.Backend.Contracts;
using Hydra.Module.Video.Backend.Data;
using Hydra.Module.Video.Backend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.Module.Video.Backend
{
    public static class SharedApiExtensions
    {
        public static IServiceCollection AddHydraModuleVideo(
            this IServiceCollection services)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            var configBuilder = new ConfigurationBuilder();

            configBuilder.AddJsonFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                "ModuleVideoSettings.json"));

            var settings = configBuilder.Build().Get<ModuleVideoConfiguration>();

            services.AddDbContext<VideoDbContext>(options =>
            {
                options.UseSqlite(settings.ConnectionString, opt =>
                {
                    opt.MigrationsAssembly(callingAssembly.GetName().FullName);
                });
            });

            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddSingleton(settings);

            JwtAuthentication.AddJwtTokenService(services, settings);

            return services;
        }
        public static IApplicationBuilder UseHydraModuleVideo(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var ctx = scope.ServiceProvider.GetRequiredService<VideoDbContext>();

            ctx.Database.EnsureCreated();

            builder.UseAuthentication();

            return builder;
        }

    }
}