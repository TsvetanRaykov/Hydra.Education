namespace Hydra.Module.Video.Backend.Controllers
{
    using Contracts;
    using Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class FileController : ApiControllerBase
    {
        private const string FileStorageDirectory = "\\Files\\";
        private readonly IFileService _fileService;
        private readonly IVideoService _videoService;

        public FileController(IFileService fileService, IVideoService videoService)
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

            var directoryPath = Environment.CurrentDirectory + FileStorageDirectory;
            var fullFilePath = directoryPath + Convert.ToBase64String(Encoding.UTF8.GetBytes(uploadVideo.FileChunk.FileNameNoPath));
            fullFilePath += Path.GetExtension(uploadVideo.FileChunk.FileNameNoPath);

            var error = await _fileService.WriteFileChunkAsync(fullFilePath, uploadVideo.FileChunk);

            if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

            if (!uploadVideo.FileChunk.IsLastChunk()) return Ok(true);

            error = await _videoService.CreateVideoAsync(uploadVideo, User?.Identity?.Name, $"Files/{Path.GetFileName(fullFilePath)}");

            if (string.IsNullOrWhiteSpace(error)) return Ok(true);

            _fileService.DeleteFile(fullFilePath);

            return BadRequest(error);

        }
    }
}