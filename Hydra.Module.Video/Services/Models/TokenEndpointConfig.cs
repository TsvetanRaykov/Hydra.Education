namespace Hydra.Module.Video.Services.Models
{
    using Microsoft.Extensions.Configuration;
    public class TokenEndpointConfig
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }

        public static TokenEndpointConfig ReadJsonFromFile(string path)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(path);

            var configuration = builder.Build();

            return configuration.Get<TokenEndpointConfig>();
        }
    }
}