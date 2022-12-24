using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hydra.Module.Video.Backend
{
    using Contracts;
    using Data;
    using Hydra.Module.Video.Backend.Authentication.Contracts;
    using Hydra.Module.Video.Backend.Authentication.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Services;
    using System;
    using System.IO;
    using System.Reflection;

    public static class SharedApiExtensions
    {
        public static IServiceCollection AddHydraModuleVideo(
            this IServiceCollection services, Action<ModuleVideoSettings> options)
        {
            var settings = ModuleVideoSettings.Validate(options);

            services.Configure(options);

            var callingAssembly = Assembly.GetCallingAssembly();

            services.AddDbContext<VideoDbContext>(builder =>
            {
                builder.UseSqlite(settings.ConnectionString, opt =>
                {
                    opt.MigrationsAssembly(callingAssembly.GetName().FullName);
                });
            });

            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IPlaylistService, PlaylistService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IStudentService, StudentService>();

            services.AddSingleton<IJwtTokenManager, JwtTokenManager>();

            return services;
        }

        public static void UseHydraModuleVideo(this IApplicationBuilder builder, Action<ModuleArguments> configAction = null)
        {
            using var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<VideoDbContext>();
            var config = scope.ServiceProvider.GetService<ModuleVideoSettings>();

            // Initialize the database only if it does not exist
            if (!dbContext.Database.GetService<IRelationalDatabaseCreator>().Exists())
            {
                dbContext.Database.Migrate();
            }

            var moduleConfig = new ModuleArguments();
            configAction?.Invoke(moduleConfig);

            var filesPath = moduleConfig.StaticFilesLocation ??
                            Path.Combine(Directory.GetCurrentDirectory(), "Files");

            if (!Directory.Exists(filesPath))
            {
                Directory.CreateDirectory(filesPath);
            }

            builder.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(filesPath),
                RequestPath = "/Files"
            });

            if (config != null) config.StaticFilesLocation = filesPath;

            builder.UseAuthentication();
        }

    }

}
