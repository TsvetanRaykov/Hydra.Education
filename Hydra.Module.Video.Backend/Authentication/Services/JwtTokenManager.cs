using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Hydra.Module.Video.Backend.Authentication.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace Hydra.Module.Video.Backend.Authentication.Services
{
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

            var key = Encoding.ASCII.GetBytes(_configuration.JwtConfig.Key);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, clientName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}