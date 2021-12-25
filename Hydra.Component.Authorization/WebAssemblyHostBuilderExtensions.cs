using System;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;

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
        /// <param name="options"></param>
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

            hostBuilder.Services.AddSingleton(async p =>
            {
                var httpClient = p.GetRequiredService<HttpClient>();
                httpClient.BaseAddress = new Uri(hostBuilder.HostEnvironment.BaseAddress);
                return await httpClient.GetFromJsonAsync<AuthOptions>("endpoints.json")
                    .ConfigureAwait(false);
            });

            return hostBuilder;
        }
    }
}