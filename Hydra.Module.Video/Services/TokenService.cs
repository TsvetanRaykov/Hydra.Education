using Hydra.Module.Video.Contracts;
using Hydra.Module.Video.Models;

namespace Hydra.Module.Video.Services
{
    using JWT;
    using JWT.Algorithms;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class TokenService : ITokenService
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cache;
        private readonly IConfiguration _configuration;
        private readonly IJsonSerializer _serializer;
        private readonly IDateTimeProvider _provider;
        private readonly IBase64UrlEncoder _urlEncoder;
        private readonly IJwtAlgorithm _algorithm;

        private readonly string _tokenEndpoint;
        private readonly string _apiKey;

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
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();

            _tokenEndpoint = $"{_configuration["Endpoints:BaseUrl"]}/{_configuration["Endpoints:Token"]}";
            _apiKey = _configuration["ApiKey"] ?? "--- hydra-joke-key ---";
        }

        public async Task<string> GetTokenAsync()
        {
            const string cacheKey = "AuthJwtToken";

            var token = _cache.GetFromCache<string>(cacheKey);

            if (!string.IsNullOrWhiteSpace(token)) return token;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Headers = { { "ApiKey", _apiKey } },
                RequestUri = new Uri(_tokenEndpoint)
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