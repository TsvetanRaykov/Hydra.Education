using System.Collections.Generic;
using System.Security.Claims;

namespace Hydra.Module.Video.Backend.Authentication.Services
{
    using Contracts;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Text;

    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly ModuleVideoSettings _configuration;

        public JwtTokenManager(ModuleVideoSettings configuration)
        {
            _configuration = configuration;
        }

        public string Authenticate(string apiKey)
        {

            var client = _configuration.ApiClients.FirstOrDefault(c => c.ApiKey.Equals(apiKey));

            if (string.IsNullOrWhiteSpace(apiKey) || client == null)
            {
                return null;
            }

            return CreateToken(client);
        }

        private string CreateToken(ApiClient client)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.JwtConfig.Key);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Claims = new Dictionary<string, object>()
            };

            foreach (var role in client.Roles)
            {
                tokenDescriptor.Claims.Add(ClaimTypes.Role, role);
            }

            tokenDescriptor.Claims.Add(ClaimTypes.Name, client.UserName);
            tokenDescriptor.Claims.Add(ClaimTypes.Email, client.UserName);

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}