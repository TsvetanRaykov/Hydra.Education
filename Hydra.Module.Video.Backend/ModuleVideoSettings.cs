namespace Hydra.Module.Video.Backend
{
    using Contracts;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Text;

    public class ModuleVideoSettings
    {
        public string ConnectionString { get; set; }
        public string StaticFilesLocation { get; set; }
        public JwtConfig JwtConfig { get; set; }
        public ApiClient[] ApiClients { get; set; } = Array.Empty<ApiClient>();

        public Action<IAddHydraModuleAuthentication> Authenticate { get; set; }

        public void AddHydraModuleJwtTokenAuthentication(IServiceCollection services)
        {
            ValidateAuthConfiguration();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var key = Encoding.ASCII.GetBytes(JwtConfig.Key);
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

        public static ModuleVideoSettings Validate(Action<ModuleVideoSettings> config)
        {
            var options = new ModuleVideoSettings();
            config(options);
            options.Validate();
            return options;
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new ArgumentNullException(nameof(ConnectionString), $"{nameof(ConnectionString)} is required.");
            if (string.IsNullOrWhiteSpace(StaticFilesLocation))
                throw new ArgumentNullException(nameof(StaticFilesLocation), $"{nameof(StaticFilesLocation)} is required.");
        }

        private void ValidateAuthConfiguration()
        {

            if (JwtConfig == null)
                throw new ArgumentNullException(nameof(JwtConfig), $"{nameof(JwtConfig)} object is required.");
            JwtConfig.Validate();

            if (ApiClients.Length == 0)
                throw new ArgumentException($"{nameof(ApiClients)} collection is empty.");

            foreach (var apiClient in ApiClients)
            {
                apiClient.Validate();
            }
        }
    }

    public class ApiClient
    {
        public string ApiKey { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string[] Roles { get; set; } = Array.Empty<string>();

        internal void Validate()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new ArgumentNullException(nameof(ApiKey), $"{nameof(ApiClient)}.{nameof(ApiKey)} is required.");
            if (string.IsNullOrWhiteSpace(UserName))
                throw new ArgumentNullException(nameof(UserName), $"{nameof(ApiClient)}.{nameof(UserName)} is required.");
            if (string.IsNullOrWhiteSpace(FullName))
                throw new ArgumentNullException(nameof(FullName), $"{nameof(ApiClient)}.{nameof(FullName)} is required.");

            if (Roles.Length == 0)
                throw new ArgumentException($"{nameof(ApiClient)}.{nameof(Roles)} collection is empty.");

            foreach (var role in Roles)
            {
                if (string.IsNullOrWhiteSpace(role))
                    throw new ArgumentNullException(nameof(role), $"{nameof(ApiClient)}.{nameof(Roles)} contains empty strings.");

            }
        }
    }

    public class JwtConfig
    {
        public string Key { get; set; }
        internal void Validate()
        {
            if (string.IsNullOrWhiteSpace(Key))
                throw new ArgumentNullException(nameof(Key), $"{nameof(JwtConfig)}.{nameof(Key)} is required.");

        }
    }

}