namespace Hydra.Module.Video
{
    using Blazorise;
    using Blazorise.Bootstrap;
    using Blazorise.Icons.FontAwesome;
    using Component.Authorization;
    using Contracts;
    using JWT;
    using JWT.Algorithms;
    using JWT.Serializers;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

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

            builder.AddHydraAuthorization(options =>
            {
                builder.Configuration.Bind(options);
            });

            builder.Services.AddTransient<BearerTokenHandler>();

            builder.Services.AddHttpClient("authorized", config =>
                {
                    config.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
                })
                .AddHttpMessageHandler<BearerTokenHandler>();

            builder.Services.AddSingleton(sp =>
                sp.GetRequiredService<IHttpClientFactory>().CreateClient("authorized"));

            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<ICacheService, CacheService>();
            builder.Services.AddSingleton<ITokenService, TokenService>();
            builder.Services.AddSingleton<IClassService, ClassService>();
            builder.Services.AddSingleton<IGroupService, GroupService>();
            builder.Services.AddSingleton<IPlaylistService, PlaylistService>();
            builder.Services.AddSingleton<IVideoService, VideoService>();
            builder.Services.AddSingleton<IStudentsService, StudentsService>();

            builder.Services.AddSingleton<IJsonSerializer, JsonNetSerializer>();
            builder.Services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
            builder.Services.AddSingleton<IBase64UrlEncoder, JwtBase64UrlEncoder>();
            builder.Services.AddSingleton<IJwtAlgorithm, HMACSHA256Algorithm>();

            await builder.Build().RunAsync();
        }
    }
}
