namespace Hydra.Component.Authorization.Services
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Security.Claims;
    using System.Threading.Tasks;

    //[tr]: 2021-10-26
    public class HostAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static readonly TimeSpan UserCacheRefreshInterval = TimeSpan.FromSeconds(60);

        private readonly NavigationManager _navigation;
        private readonly HttpClient _client;
        private readonly ILogger<HostAuthenticationStateProvider> _logger;

        private DateTimeOffset _userLastCheck = DateTimeOffset.FromUnixTimeSeconds(0);
        private ClaimsPrincipal _cachedUser = new(new ClaimsIdentity());

        private readonly AuthOptions _authOptions;

        public HostAuthenticationStateProvider(NavigationManager navigation, HttpClient client, ILogger<HostAuthenticationStateProvider> logger, IOptions<AuthOptions> authOptions)
        {
            _navigation = navigation;
            _client = client;
            _logger = logger;
            _authOptions = authOptions.Value;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync() => new AuthenticationState(await GetUser(useCache: true));

        public async void SignIn(string customReturnUrl = null)
        {
            var returnUrl = customReturnUrl != null ? _navigation.ToAbsoluteUri(customReturnUrl).ToString() : null;
            var encodedReturnUrl = Uri.EscapeDataString(returnUrl ?? _navigation.Uri);
            var logInUrl = $"{_authOptions.Endpoints.BaseUrl.OriginalString}/{_authOptions.Endpoints.SignIn}?returnUrl={encodedReturnUrl}";
            _navigation.NavigateTo(logInUrl, true);
        }

        public async void SignOut()
        {
            _navigation.NavigateTo($"{_authOptions.Endpoints.BaseUrl.OriginalString}/{_authOptions.Endpoints.SignOut}", true);
        }

        private async ValueTask<ClaimsPrincipal> GetUser(bool useCache = false)
        {
            var now = DateTimeOffset.Now;
            if (useCache && now < _userLastCheck + UserCacheRefreshInterval)
            {
                _logger.LogDebug("Taking user from cache");
                return _cachedUser;
            }

            _logger.LogDebug("Fetching user");
            _cachedUser = await FetchUser();
            _userLastCheck = now;

            return _cachedUser;
        }

        private async Task<ClaimsPrincipal> FetchUser()
        {
            UserInfo user = null;

            try
            {
                user = await _client.GetFromJsonAsync<UserInfo>($"{_authOptions.Endpoints.BaseUrl.OriginalString}/{_authOptions.Endpoints.User}");
            }
            catch (Exception exc)
            {
                _logger.LogWarning(exc, "Fetching user failed.");
            }

            return BuildClaimsPrincipal(user);
        }

        private static ClaimsPrincipal BuildClaimsPrincipal(UserInfo userInfo)
        {
            if (userInfo is not { IsAuthenticated: true })
            {
                return new ClaimsPrincipal(new ClaimsIdentity());
            }

            var identity = new ClaimsIdentity(
                nameof(HostAuthenticationStateProvider),
                userInfo.NameClaimType,
                userInfo.RoleClaimType);

            if (userInfo.Claims == null) return new ClaimsPrincipal(identity);

            foreach (var claim in userInfo.Claims)
            {
                identity.AddClaim(new Claim(claim.Type, claim.Value));
            }

            return new ClaimsPrincipal(identity);
        }
    }
}
