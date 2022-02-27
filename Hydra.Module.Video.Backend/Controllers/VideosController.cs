using Microsoft.IdentityModel.Tokens;

namespace Hydra.Module.Video.Backend.Controllers
{
    using Contracts;
    using Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class VideosController : ApiControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IVideoService _videoService;

        public VideosController(IFileService fileService, IVideoService videoService, ModuleVideoSettings config) : base(config)
        {
            _fileService = fileService;
            _videoService = videoService;
        }

        [Authorize(Roles = "Admin, Trainer")]
        [HttpPost]
        public async Task<ActionResult<bool>> UploadVideo([FromBody] VideoRequestDto uploadVideo)
        {

            if (uploadVideo?.FileChunk?.FileNameNoPath == null)
            {
                return BadRequest($"{nameof(uploadVideo.FileChunk.FileNameNoPath)} is missing.");
            }

            var fullFilePath = Path.Combine(Configuration.StaticFilesLocation,
                Base64UrlEncoder.Encode(uploadVideo.FileChunk.FileNameNoPath));


            fullFilePath += Path.GetExtension(uploadVideo.FileChunk.FileNameNoPath);

            var error = await _fileService.WriteFileChunkAsync(fullFilePath, uploadVideo.FileChunk);

            if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

            if (!uploadVideo.FileChunk.IsLastChunk()) return Ok(true);

            error = await _videoService.CreateVideoAsync(uploadVideo, User?.Identity?.Name, fullFilePath);

            if (string.IsNullOrWhiteSpace(error)) return Ok(true);

            _fileService.DeleteFile(fullFilePath);

            return BadRequest(error);

        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult> DeleteVideo(int id)
        {
            var video = await _videoService.GetVideoAsync(id);

            if (video == null) return BadRequest("Video not found");

            var fullFilePath = Path.Combine(Configuration.StaticFilesLocation,
                Path.GetFileName(video.Url) ?? string.Empty);

            if (System.IO.File.Exists(fullFilePath))
            {
                try
                {
                    System.IO.File.Delete(fullFilePath);
                }
                catch (IOException e)
                {
                    return BadRequest(e.Message);
                }
            }

            var error = await _videoService.DeleteVideoAsync(id);
            if (string.IsNullOrWhiteSpace(error)) return Ok(true);

            return BadRequest(error);
        }

        [HttpPost("in")]
        public async Task<IEnumerable<VideoResponseDto>> GetVideos([FromBody] int[] playlists)
        {
            return await _videoService.GetVideosInPlayListsAsync(playlists);
        }

        [HttpGet("{id:int}")]
        public async Task<VideoResponseDto> GetVideo(int id)
        {
            return await _videoService.GetVideoAsync(id);
        }

        [HttpGet("owner/{id}")]
        public async Task<IEnumerable<VideoResponseDto>> GetVideosOfTrainer(string id)
        {
            return await _videoService.GetVideosByUploader(id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<IEnumerable<VideoResponseDto>> GetAllVideos()
        {
            return await _videoService.GetAllVideosAsync();
        }
    }
}