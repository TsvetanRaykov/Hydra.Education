using Microsoft.Extensions.Options;

namespace Hydra.Module.Video.Backend.Controllers
{
    using Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ClassesController : ApiControllerBase
    {
        private readonly IClassService _classService;
        private readonly IFileService _fileService;

        public ClassesController(IClassService classService, IFileService fileService, IOptions<ModuleVideoSettings> config) : base(config)
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
            string imagePath = null;
            if (newClassRequest.Image is { Length: > 0 })
            {
                imagePath = BuildImagePath(newClassRequest.Name);
                var fileSaveError = await SaveImage(_fileService, imagePath, newClassRequest.Image);

                if (!string.IsNullOrWhiteSpace(fileSaveError))
                    return BadRequest(false);
            }

            var resultError = await _classService.CreateClassAsync(newClassRequest.Name, newClassRequest.Description, BuildImageUrl(imagePath), User.Identity?.Name);

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
                var fileSaveError = await SaveImage(_fileService, imagePath, classUpdate.Image);

                if (!string.IsNullOrWhiteSpace(fileSaveError))
                    return BadRequest(false);

                classUpdate.ImageUrl = BuildImageUrl(imagePath);
            }

            var resultError = await _classService.UpdateClassAsync(classUpdate.Id, classUpdate.Name, classUpdate.Description,
               classUpdate.ImageUrl);

            if (string.IsNullOrWhiteSpace(resultError))
                return Ok(true);

            return BadRequest(false);
        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> Delete(int id)
        {
            var error = await _classService.DeleteClassAsync(id);

            if (string.IsNullOrWhiteSpace(error))
                return Ok(true);

            return BadRequest(false);
        }
    }
}
