using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using Hydra.Component.Authorization.Authorization;

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
        public static WebAssemblyHostBuilder AddHydraAuthorization(this WebAssemblyHostBuilder hostBuilder,
            Action<AuthOptions> options = null)
        {

            hostBuilder.Services.AddSingleton<TempUser>(provider => null);

            hostBuilder.Services.AddOptions();
            hostBuilder.Services.AddAuthorizationCore();
            hostBuilder.Services.TryAddSingleton<AuthenticationStateProvider, HostAuthenticationStateProvider>();
            hostBuilder.Services.TryAddSingleton(serviceProvider =>
                (HostAuthenticationStateProvider)serviceProvider.GetRequiredService<AuthenticationStateProvider>());
            hostBuilder.Services.AddTransient<AuthorizedHandler>();

            if (options == null)
            {
                hostBuilder.Services.AddHttpClient("default",
                    client => client.BaseAddress = new Uri(hostBuilder.HostEnvironment.BaseAddress));
                hostBuilder.Services.AddTransient(sp =>
                    sp.GetRequiredService<IHttpClientFactory>().CreateClient("default"));

                return hostBuilder;
            }

            var authOptions = new AuthOptions();
            options(authOptions);

            if (string.IsNullOrWhiteSpace(authOptions.HttpClientName))
            {
                throw new ArgumentNullException(nameof(options), $"{nameof(authOptions.HttpClientName)} is not provided");
            }

            hostBuilder.Services.AddHttpClient(authOptions.HttpClientName,
                    client => client.BaseAddress = new Uri(authOptions.HttpClientBaseAddress ?? hostBuilder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<AuthorizedHandler>();

            return hostBuilder;
        }

        public static WebAssemblyHostBuilder AddHydraAuthorizationDeveloper(this WebAssemblyHostBuilder hostBuilder, TempUser user)
        {
            hostBuilder.Services.AddSingleton(user);
            return hostBuilder;
        }
    }
}