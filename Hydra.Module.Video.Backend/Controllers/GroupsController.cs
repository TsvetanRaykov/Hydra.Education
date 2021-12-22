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
        public GroupsController(IGroupService groupService, IFileService fileService, ModuleVideoSettings config) : base(config)
        {
            _groupService = groupService;
            _fileService = fileService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> Post(GroupRequestDto groupRequestDto)
        {
            var imagePath = BuildImagePath(groupRequestDto.Name);

            var file = new FileChunk
            {
                Data = groupRequestDto.Image,
                Offset = 0,
                FirstChunk = true
            };

            var fileSaveError = await _fileService.WriteFileChunkAsync(imagePath, file);

            if (!string.IsNullOrWhiteSpace(fileSaveError))
                return BadRequest(false);

            var resultError = await _groupService.CreateGroupAsync(groupRequestDto.Name, groupRequestDto.Description, BuildImageUrl(imagePath), groupRequestDto.ClassId);

            if (string.IsNullOrWhiteSpace(resultError))
                return Ok(true);

            return BadRequest(false);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<GroupResponseDto>> Get(int id)
        {
            var videoClass = await _groupService.GetGroupAsync(id);
            return Ok(videoClass);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> Put(GroupRequestDto groupUpdate)
        {
            if (groupUpdate.Image != null)
            {
                var imagePath = BuildImagePath(groupUpdate.Name);
                var fileSaveError = await SaveImage(_fileService, imagePath, groupUpdate.Image);

                if (!string.IsNullOrWhiteSpace(fileSaveError))
                    return BadRequest(false);

                groupUpdate.ImageUrl = BuildImageUrl(imagePath);
            }

            var resultError = await _groupService.UpdateGroupAsync(groupUpdate.Id, groupUpdate.Name, groupUpdate.Description,
                groupUpdate.ImageUrl);

            if (string.IsNullOrWhiteSpace(resultError))
                return Ok(true);

            return BadRequest(false);
        }
    }
}