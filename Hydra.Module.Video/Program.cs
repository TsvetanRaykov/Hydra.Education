using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Hydra.Component.Authorization;
using Hydra.Component.Authorization.Models;
using Hydra.Module.Video.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hydra.Module.Video.Services.Contracts;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace Hydra.Module.Video
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddBlazorise(options =>
                {
                    options.ChangeTextOnKeyPress = true;
                })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();

            builder.AddHydraAuthorization()
                //.AddHydraAuthorizationDeveloper(new TempUser
                //{
                //    Name = "Demo User",
                //    Roles = new List<string> { "Student", "Trainer" }
                //})
                ;

            builder.Services.AddSingleton<BearerTokenHandler>();

            builder.Services.AddHttpClient("authorized", config =>
                 {
                     var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                     config.BaseAddress = new Uri(configuration.GetValue<string>("ApiBaseUrl"));
                 })
                 .AddHttpMessageHandler<BearerTokenHandler>();

            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<ICacheService, CacheService>();
            builder.Services.AddSingleton<ITokenService, TokenService>();

            builder.Services.AddSingleton<IJsonSerializer, JsonNetSerializer>();
            builder.Services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
            builder.Services.AddSingleton<IBase64UrlEncoder, JwtBase64UrlEncoder>();
            builder.Services.AddSingleton<IJwtAlgorithm, HMACSHA256Algorithm>();

            
            await builder.Build().RunAsync();
        }
    }
}
