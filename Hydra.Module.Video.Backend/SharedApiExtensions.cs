namespace Hydra.Module.Video.Backend
{
    using Contracts;
    using Data;
    using Hydra.Module.Video.Backend.Authentication.Contracts;
    using Hydra.Module.Video.Backend.Authentication.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.IdentityModel.Tokens;
    using Services;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public static class SharedApiExtensions
    {
        public static IServiceCollection AddHydraModuleVideo(
            this IServiceCollection services, Action<IAddHydraModuleAuthentication> auth = null)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            var settings = ModuleVideoConfiguration();

            services.AddDbContext<VideoDbContext>(options =>
            {
                options.UseSqlite(settings.ConnectionString, opt =>
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

            services.AddSingleton(settings);
            services.AddSingleton<IJwtTokenManager, JwtTokenManager>();

            auth?.Invoke(new HydraModuleAuthentication());

            return services;
        }

        public static void UseHydraModuleVideo(this IApplicationBuilder builder, Action<ModuleArguments> configAction = null)
        {
            using var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<VideoDbContext>();
            var config = scope.ServiceProvider.GetService<ModuleVideoSettings>();

            dbContext.Database.EnsureCreated();

            var moduleConfig = new ModuleArguments();
            configAction?.Invoke(moduleConfig);

            var staticFilesPath = moduleConfig.StaticFilesLocation ?? Path.Combine(Directory.GetCurrentDirectory(), "Files");
            var filesPath = Path.Combine(staticFilesPath, "ModuleVideo");

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

        private static ModuleVideoSettings ModuleVideoConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                "module-video-settings.json"));

            var settings = configBuilder.Build().Get<ModuleVideoSettings>();
            return settings;
        }

        private class HydraModuleAuthentication : IAddHydraModuleAuthentication
        {
            public void AddHydraModuleJwtTokenAuthentication(IServiceCollection services)
            {
                var settings = ModuleVideoConfiguration();

                services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        var key = Encoding.ASCII.GetBytes(settings.JwtConfig.Key);
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateLifetime = true,
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ClockSkew = TimeSpan.Zero
                        };
                    });
            }
        }

    }

}
