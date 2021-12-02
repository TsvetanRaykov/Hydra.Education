using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Hydra.Module.Video.Backend.Controllers
{
    using Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class ClassesController : ApiControllerBase
    {
        private readonly ILogger<ClassesController> _logger;
        private readonly IClassService _classService;
        private readonly IFileService _fileService;

        public ClassesController(ILogger<ClassesController> logger, IClassService classService, IFileService fileService)
        {
            _logger = logger;
            _classService = classService;
            _fileService = fileService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<ClassResponseDto>>> GetUserClasses(string userId)
        {
            //var token = await HttpContext.GetTokenAsync("access_token");

            var classes = await _classService.GetClassesAsync(userId);

            return Ok(classes);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> Post(ClassRequestDto newClassRequest)
        {
            if (User.Identity == null)
            {
                return BadRequest("Authentication error");
            }

            var imagePath = $"Files/{newClassRequest.Name}-{DateTime.Now.Ticks}.png";

            var filePath = Path.Combine(Environment.CurrentDirectory, imagePath);

            var file = new FileChunk
            {
                Data = newClassRequest.Image,
                Offset = 0,
                FirstChunk = true
            };

            var fileSaveError = await _fileService.WriteFileChunkAsync(filePath, file);

            if (!string.IsNullOrWhiteSpace(fileSaveError))
                return BadRequest(false);

            var resultError = await _classService.CreateClassAsync(newClassRequest.Name, newClassRequest.Description, $"/{imagePath}", User.Identity.Name);

            if (string.IsNullOrWhiteSpace(resultError))
                return Ok(true);

            return BadRequest(false);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ClassResponseDto>> Get(int id)
        {
            var videoClass = await _classService.GetClassAsync(id);

            return Ok(videoClass);
        }
    }
}
