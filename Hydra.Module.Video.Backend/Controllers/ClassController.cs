using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Hydra.Module.Video.Backend.Controllers
{
    using Contracts;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class ClassController : ApiControllerBase
    {
        private readonly ILogger<ClassController> _logger;
        private readonly IClassService _classService;
        private readonly IFileService _fileService;

        public ClassController(ILogger<ClassController> logger, IClassService classService, IFileService fileService)
        {
            _logger = logger;
            _classService = classService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            return Ok($"WORKS: {token}");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> Post(ClassDto newClass)
        {
            if (User.Identity == null)
            {
                return BadRequest("Authentication error");
            }

            var imagePath = $"Files/{newClass.Name}-{DateTime.Now.Ticks}.png";

            var filePath = Path.Combine(Environment.CurrentDirectory, imagePath);

            var file = new FileChunk
            {
                Data = newClass.Image,
                Offset = 0,
                FirstChunk = true
            };

            var fileSaveError = await _fileService.WriteFileChunkAsync(filePath, file);

            if (!string.IsNullOrWhiteSpace(fileSaveError))
                return BadRequest(false);

            var resultError = await _classService.CreateClass(newClass.Name, newClass.Description, imagePath, User.Identity.Name);

            if (string.IsNullOrWhiteSpace(resultError))
                return Ok(true);

            return BadRequest(false);
        }
    }
}
