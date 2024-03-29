﻿using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Hydra.Component.Authorization.Services
{
    //[tr]: 2021-10-25
    public class AuthorizedHandler : DelegatingHandler
    {
        private readonly HostAuthenticationStateProvider _authenticationStateProvider;

        public AuthorizedHandler(HostAuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            HttpResponseMessage responseMessage;
            if (authState.User.Identity is { IsAuthenticated: false })
            {
                responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            else
            {
                responseMessage = await base.SendAsync(request, cancellationToken);
            }

            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationStateProvider.SignIn();
            }

            return responseMessage;
        }
    }
}
