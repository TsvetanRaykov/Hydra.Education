namespace Hydra.Module.Video.Backend.Authentication.Services
{
    using Contracts;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;

    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly ModuleVideoSettings _configuration;
        private readonly string _tokenGeneratorKey;

        public JwtTokenManager(IOptions<ModuleVideoSettings> configuration)
        {
            _configuration = configuration.Value;
            _tokenGeneratorKey = _configuration.JwtConfig?.Key;
        }

        public string Authenticate(string apiKey)
        {

            if (string.IsNullOrWhiteSpace(_tokenGeneratorKey))
            {
                return CreateToken(new ApiClient(), "--- hydra-token-joke ---");
            }

            var client = _configuration.ApiClients.FirstOrDefault(c => c.ApiKey.Equals(apiKey));

            if (string.IsNullOrWhiteSpace(apiKey) || client == null)
            {
                return null;
            }

            return CreateToken(client);
        }

        private string CreateToken(ApiClient client)
        {
            return CreateToken(client, _tokenGeneratorKey);
        }

        private static string CreateToken(ApiClient client, string key)
        {
            var keyByte = Encoding.ASCII.GetBytes(key);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyByte), SecurityAlgorithms.HmacSha256Signature),
                Claims = new Dictionary<string, object>()
            };

            foreach (var role in client.Roles)
            {
                tokenDescriptor.Claims.Add(ClaimTypes.Role, role);
            }

            tokenDescriptor.Claims.Add(ClaimTypes.GivenName, client.FullName);
            tokenDescriptor.Claims.Add(ClaimTypes.Name, client.UserName);
            tokenDescriptor.Claims.Add(ClaimTypes.Email, client.UserName);

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}