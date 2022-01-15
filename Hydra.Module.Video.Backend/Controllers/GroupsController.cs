namespace Hydra.Module.Video.Backend.Controllers
{
    using Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Threading.Tasks;

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
            var videoGroup = await _groupService.GetGroupAsync(id);
            return Ok(videoGroup);
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

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> Delete(int id)
        {
            var error = await _groupService.DeleteGroupAsync(id);

            if (string.IsNullOrWhiteSpace(error))
                return Ok(true);

            return BadRequest(false);
        }

        [HttpPost("{id:int}/users")]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> SetUsers(int id, [FromBody] string[] usersIds)
        {
            var resultError = await _groupService.SetUsersAsync(id, usersIds);

            if (string.IsNullOrWhiteSpace(resultError))
                return Ok(true);

            return BadRequest(false);
        }

        [HttpPost("{groupId:int}/playlists/{playlistId}")]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> AddPlaylist(int groupId, int playlistId)
        {
            var resultError = await _groupService.AddPlaylist(groupId, playlistId);

            if (string.IsNullOrWhiteSpace(resultError))
                return Ok(true);

            return BadRequest(false);
        }

        [HttpDelete("{groupId:int}/playlists/{playlistId}")]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> RemovePlaylist(int groupId, int playlistId)
        {
            var resultError = await _groupService.RemovePlaylist(groupId, playlistId);

            if (string.IsNullOrWhiteSpace(resultError))
                return Ok(true);

            return BadRequest(false);
        }
    }
}