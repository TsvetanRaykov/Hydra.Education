using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hydra.Module.Video.Contracts;

namespace Hydra.Module.Video.Services
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenProvider;

        public BearerTokenHandler(ITokenService tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains("Authorization"))
            {
                return await base.SendAsync(request, cancellationToken);
            }
            var token = await _tokenProvider.GetTokenAsync();
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }

    }
}