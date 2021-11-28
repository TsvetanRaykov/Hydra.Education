using System.Threading.Tasks;
using Hydra.Module.Video.Backend.Contracts;

namespace Hydra.Module.Video.Backend.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System;
    using System.IO;

    public class FileController : ApiControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<bool> UploadFileChunk([FromBody] FileChunk fileChunk)
        {

            var filePath = Environment.CurrentDirectory + "\\Files\\";
            var fileName = filePath + fileChunk.FileNameNoPath;

            var result = await _fileService.WriteFileChunkAsync(fileName, fileChunk);

            return string.IsNullOrWhiteSpace(result);

        }
    }
}