using Hydra.Module.Video.Backend.Authentication.Contracts;
using Hydra.Module.Video.Backend.Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Module.Video.Backend.Controllers
{
    [ApiController]
    [Route("api/video/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenManager _jwtTokenManager;
        public TokenController(IJwtTokenManager jwtTokenManager)
        {
            _jwtTokenManager = jwtTokenManager;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate(ClientCredential credential)
        {
            var token = _jwtTokenManager.Authenticate(credential.ClientId, credential.ClientSecret);
            if (string.IsNullOrWhiteSpace(token))
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}