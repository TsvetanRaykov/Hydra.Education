using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Hydra.Component.Authorization;
using Hydra.Module.Video.Contracts;
using Hydra.Module.Video.Services;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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

            builder.AddHydraAuthorization();

            builder.Services.AddSingleton<BearerTokenHandler>();

            builder.Services.AddHttpClient("authorized", config =>
                {
                    var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                    config.BaseAddress = new Uri(configuration.GetValue<string>("ApiBaseUrl"));
                })
                .AddHttpMessageHandler<BearerTokenHandler>();

            builder.Services.AddSingleton(sp =>
                sp.GetRequiredService<IHttpClientFactory>().CreateClient("authorized"));

            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<ICacheService, CacheService>();
            builder.Services.AddSingleton<ITokenService, TokenService>();
            builder.Services.AddSingleton<IFileService, FileService>();
            builder.Services.AddSingleton<IClassService, ClassService>();
            builder.Services.AddSingleton<IGroupService, GroupService>();
            builder.Services.AddSingleton<IPlaylistService, PlaylistService>();

            builder.Services.AddSingleton<IJsonSerializer, JsonNetSerializer>();
            builder.Services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
            builder.Services.AddSingleton<IBase64UrlEncoder, JwtBase64UrlEncoder>();
            builder.Services.AddSingleton<IJwtAlgorithm, HMACSHA256Algorithm>();


            await builder.Build().RunAsync();
        }
    }
}
