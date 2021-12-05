namespace Hydra.Module.Video.Backend.Controllers
{
    using Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public class ClassesController : ApiControllerBase
    {
        private readonly IClassService _classService;
        private readonly IFileService _fileService;

        public ClassesController(IClassService classService, IFileService fileService)
        {
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
            var imagePath = BuildImagePath(newClassRequest.Name);
            var fileSaveError = await SaveImage(imagePath, newClassRequest.Image);

            if (!string.IsNullOrWhiteSpace(fileSaveError))
                return BadRequest(false);

            var resultError = await _classService.CreateClassAsync(newClassRequest.Name, newClassRequest.Description, $"/{imagePath}", User.Identity?.Name);

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

        [HttpPut]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> Put(ClassRequestDto classUpdate)
        {
            if (classUpdate.Image != null)
            {
                var imagePath = BuildImagePath(classUpdate.Name);
                var fileSaveError = await SaveImage(imagePath, classUpdate.Image);

                if (!string.IsNullOrWhiteSpace(fileSaveError))
                    return BadRequest(false);

                classUpdate.ImageUrl = imagePath;
            }

            var resultError = await _classService.UpdateClassAsync(classUpdate.Id, classUpdate.Name, classUpdate.Description,
               $"/{classUpdate.ImageUrl}");

            if (string.IsNullOrWhiteSpace(resultError))
                return Ok(true);

            return BadRequest(false);
        }


        private static string BuildImagePath(string className)
        {
            var imageName = className.ToLower().Replace(' ', '_');
            // $"{name}-{DateTime.Now.Ticks}.png";
            var imagePath = $"Files/{imageName}.png";
            return imagePath;
        }

        private async Task<string> SaveImage(string imagePath, byte[] imageData)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, imagePath);

            var file = new FileChunk
            {
                Data = imageData,
                Offset = 0,
                FirstChunk = true
            };

            var fileSaveError = await _fileService.WriteFileChunkAsync(filePath, file);

            return fileSaveError;

        }
    }
}
