namespace Hydra.Module.Video.Backend.Controllers
{
    using Hydra.Module.Video.Backend.Authentication.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Authenticate()
        {
            if (!HttpContext.Request.Headers.TryGetValue("ApiKey", out var extractedApiKey))
            {
                return Unauthorized("Api Key was not provided");
            }

            var token = _jwtTokenManager.Authenticate(extractedApiKey);
            
            if (string.IsNullOrWhiteSpace(token))
            {
                return Unauthorized("Api Key is not valid");
            }

            return Ok(token);
        }
    }
}