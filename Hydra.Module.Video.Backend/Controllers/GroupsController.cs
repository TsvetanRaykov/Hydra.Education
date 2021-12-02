using System;
using System.IO;
using System.Threading.Tasks;
using Hydra.Module.Video.Backend.Contracts;
using Hydra.Module.Video.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Module.Video.Backend.Controllers
{
    public class GroupsController : ApiControllerBase
    {

        private readonly IGroupService _groupService;
        private readonly IFileService _fileService;
        public GroupsController(IGroupService groupService, IFileService fileService)
        {
            _groupService = groupService;
            _fileService = fileService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> Post(GroupRequestDto groupRequestDto)
        {
            if (User.Identity == null)
            {
                return BadRequest("Authentication error");
            }

            var imagePath = $"Files/{groupRequestDto.Name}-{DateTime.Now.Ticks}.png";

            var filePath = Path.Combine(Environment.CurrentDirectory, imagePath);

            var file = new FileChunk
            {
                Data = groupRequestDto.Image,
                Offset = 0,
                FirstChunk = true
            };

            var fileSaveError = await _fileService.WriteFileChunkAsync(filePath, file);

            if (!string.IsNullOrWhiteSpace(fileSaveError))
                return BadRequest(false);

            var resultError = await _groupService.CreateVideoGroupAsync(groupRequestDto.Name, groupRequestDto.Description, $"/{imagePath}", groupRequestDto.ClassId);

            if (string.IsNullOrWhiteSpace(resultError))
                return Ok(true);

            return BadRequest(false);
        }
    }
}