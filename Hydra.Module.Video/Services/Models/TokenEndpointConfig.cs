using Microsoft.Extensions.Configuration;

namespace Hydra.Module.Video.Services.Models
{
    public class TokenEndpointConfig
    {
        public string Url { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CryptoKey { get; set; }

        public static TokenEndpointConfig ReadJsonFromFile(string path)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(path);

            var configuration = builder.Build();

            return configuration.Get<TokenEndpointConfig>();
        }
    }
}