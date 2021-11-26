namespace Hydra.Module.Video.Services
{
    using Contracts;
    using JWT;
    using JWT.Algorithms;
    using Microsoft.Extensions.Configuration;
    using Models;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class TokenService : ITokenService
    {
        private readonly TokenEndpointConfig _config;
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cache;

        private readonly IJsonSerializer _serializer;
        private readonly IDateTimeProvider _provider;
        private readonly IBase64UrlEncoder _urlEncoder;
        private readonly IJwtAlgorithm _algorithm;

        public TokenService(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ICacheService cache,
            IJsonSerializer serializer,
            IDateTimeProvider provider,
            IBase64UrlEncoder urlEncoder,
            IJwtAlgorithm algorithm)
        {
            _cache = cache;
            _serializer = serializer;
            _provider = provider;
            _urlEncoder = urlEncoder;
            _algorithm = algorithm;
            _config = configuration.GetSection("TokenEndpoint").Get<TokenEndpointConfig>();
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> GetTokenAsync()
        {
            const string cacheKey = "AuthJwtToken";

            var token = _cache.GetFromCache<string>(cacheKey);

            if (!string.IsNullOrWhiteSpace(token)) return token;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Headers = { { "ApiKey", _config.ApiKey } },
                RequestUri = new Uri(_config.Url)
            };

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK) return token;

            token = await response.Content.ReadAsStringAsync();
            var expire = GetExpireTime(token);
            _cache.SetCache(cacheKey, token, new DateTimeOffset(expire));

            return token;
        }

        private DateTime GetExpireTime(string accessToken)
        {
            try
            {
                IJwtValidator validator = new JwtValidator(_serializer, _provider);
                IJwtDecoder decoder = new JwtDecoder(_serializer, validator, _urlEncoder, _algorithm);
                var token = decoder.DecodeToObject<JwtToken>(accessToken);
                var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(token.exp);
                return dateTimeOffset.LocalDateTime;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

    }
}