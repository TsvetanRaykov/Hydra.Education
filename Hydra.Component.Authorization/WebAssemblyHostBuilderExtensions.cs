using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hydra.Component.Authorization
{
    using Services;
    // [tr]:2021-10-26
    public static class WebAssemblyHostBuilderExtensions
    {
        /// <summary>
        /// Adds Hydra User Authorization interaction into a Hydra WASM Module
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static WebAssemblyHostBuilder AddHydraAuthorization(this WebAssemblyHostBuilder hostBuilder)
        {

            hostBuilder.Services.AddOptions();
            hostBuilder.Services.AddAuthorizationCore();
            hostBuilder.Services.TryAddSingleton<AuthenticationStateProvider, HostAuthenticationStateProvider>();
            hostBuilder.Services.TryAddSingleton(serviceProvider =>
                (HostAuthenticationStateProvider)serviceProvider.GetRequiredService<AuthenticationStateProvider>());

            hostBuilder.Services.AddTransient<AuthorizedHandler>();

            hostBuilder.Services.AddHttpClient("default")
                .AddHttpMessageHandler<AuthorizedHandler>();

            hostBuilder.Services.AddTransient(sp =>
                sp.GetRequiredService<IHttpClientFactory>().CreateClient("default"));

            return hostBuilder;
        }

    }
}