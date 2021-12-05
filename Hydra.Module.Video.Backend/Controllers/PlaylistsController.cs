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

        public PlaylistsController(IFileService fileService, IPlaylistService playlistService)
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
    }
}