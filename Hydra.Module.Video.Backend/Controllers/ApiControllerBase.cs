namespace Hydra.Module.Video.Backend.Controllers
{
    using Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    [ApiController]
    [Authorize]
    [Route("api/video/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly ModuleVideoSettings Configuration;

        protected ApiControllerBase(ModuleVideoSettings configuration)
        {
            Configuration = configuration;
        }

        protected static string BuildImagePath(string className)
        {
            var imageName = Convert.ToBase64String(Encoding.UTF8.GetBytes(className));

            // $"{name}-{DateTime.Now.Ticks}.png";
            var imagePath = $"Files/{imageName}.png";
            return imagePath;
        }

        protected async Task<string> SaveImage(IFileService fileService, string imagePath, byte[] imageData)
        {
            var filePath = Path.Combine(Configuration.StaticFilesLocation, imagePath);

            var file = new FileChunk
            {
                Data = imageData,
                Offset = 0,
                FirstChunk = true
            };

            var fileSaveError = await fileService.WriteFileChunkAsync(filePath, file);

            return fileSaveError;

        }
    }
}