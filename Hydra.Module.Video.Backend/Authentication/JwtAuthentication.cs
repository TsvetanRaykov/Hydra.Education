namespace Hydra.Module.Video.Backend.Authentication
{
    using Contracts;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Services;
    using System;
    using System.Text;

    public static class JwtAuthentication
    {
        public static void AddJwtTokenService(IServiceCollection services, ModuleVideoConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    var key = Encoding.ASCII.GetBytes(configuration.JwtConfig.Key);
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

            services.AddSingleton<IJwtTokenManager, JwtTokenManager>();
        }
    }
}