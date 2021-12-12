using System.Collections.Generic;
using System.Threading.Tasks;
using Hydra.Module.Video.Backend.Contracts;
using Hydra.Module.Video.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Module.Video.Backend.Controllers
{
    public class PlaylistsController : ApiControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IPlaylistService _playlistService;

        public PlaylistsController(IFileService fileService, IPlaylistService playlistService, ModuleVideoSettings config) : base(config)
        {
            _fileService = fileService;
            _playlistService = playlistService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> Post(PlaylistRequestDto newPlaylist)
        {
            var imagePath = BuildImagePath(newPlaylist.Name);
            var fileSaveError = await SaveImage(_fileService, imagePath, newPlaylist.Image);

            if (!string.IsNullOrWhiteSpace(fileSaveError))
                return BadRequest(false);

            var resultError = await _playlistService.CreatePlaylistAsync(newPlaylist.Name, newPlaylist.Description, $"/{imagePath}", User.Identity?.Name);

            if (string.IsNullOrWhiteSpace(resultError))
                return Ok(true);

            return BadRequest(false);
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<PlaylistResponseDto>>> GetUserClasses(string userId)
        {
            var response = await _playlistService.GetPlayListsAsync(userId);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<PlaylistResponseDto>> Get(int id)
        {
            var videoClass = await _playlistService.GetPlaylistAsync(id);
            return Ok(videoClass);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> Put(PlaylistRequestDto playlistUpdate)
        {
            if (playlistUpdate.Image != null)
            {
                var imagePath = BuildImagePath(playlistUpdate.Name);
                var fileSaveError = await SaveImage(_fileService, imagePath, playlistUpdate.Image);

                if (!string.IsNullOrWhiteSpace(fileSaveError))
                    return BadRequest(false);

                playlistUpdate.ImageUrl = imagePath;
            }

            var resultError = await _playlistService.UpdatePlaylistAsync(playlistUpdate.Id, playlistUpdate.Name, playlistUpdate.Description,
                $"/{playlistUpdate.ImageUrl}");

            if (string.IsNullOrWhiteSpace(resultError))
                return Ok(true);

            return BadRequest(false);
        }
    }
}