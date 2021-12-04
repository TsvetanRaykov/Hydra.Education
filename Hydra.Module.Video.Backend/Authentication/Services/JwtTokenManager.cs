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
        private readonly ModuleVideoConfiguration _configuration;

        public JwtTokenManager(ModuleVideoConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Authenticate(string clientName, string clientSecret)
        {
            if (!Data.Clients.Any(c => c.Key.Equals(clientName) && c.Value.Equals(clientSecret)))
            {
                return null;
            }

            return CreateToken();
        }

        public string Authenticate(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey) || apiKey != _configuration.ApiKey)
            {
                return null;
            }

            return CreateToken();
        }

        private string CreateToken()
        {
            var key = Encoding.ASCII.GetBytes(_configuration.JwtConfig.Key);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Claims = new Dictionary<string, object>()
            };

            tokenDescriptor.Claims.Add(ClaimTypes.Name, "demo@demo.com");
            tokenDescriptor.Claims.Add(ClaimTypes.Email, "demo@demo.com");
            tokenDescriptor.Claims.Add(ClaimTypes.Role, "Trainer");

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}