using Hydra.Module.Video.Backend.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Hydra.Module.Video.Backend.Controllers
{
    public class ClassController : ModuleControllerBase
    {
        private readonly ILogger<ClassController> _logger;
        private readonly IClassService _classService;

        public ClassController(ILogger<ClassController> logger, IClassService classService)
        {
            _logger = logger;
            _classService = classService;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            return Ok($"WORKS: {token}");
        }
    }
}
