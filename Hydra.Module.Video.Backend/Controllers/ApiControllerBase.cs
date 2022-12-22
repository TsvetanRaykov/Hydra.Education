namespace Hydra.Module.Video.Backend.Controllers
{
    using Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Models;
    using System.IO;
    using System.Threading.Tasks;

    [ApiController]
    [Authorize]
    [Route("api/video/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly ModuleVideoSettings Configuration;

        protected ApiControllerBase(IOptions<ModuleVideoSettings> configuration)
        {
            Configuration = configuration.Value;
        }

        protected string BuildImagePath(string imageName)
        {
            var newName = Base64UrlEncoder.Encode(imageName);
            var imagePath = Path.Combine(Configuration.StaticFilesLocation, $"{newName}.png");

            return imagePath;
        }

        protected string BuildImageUrl(string absoluteLocalPath)
        {
            if (string.IsNullOrEmpty(absoluteLocalPath)) return null;
            var imagePath = absoluteLocalPath.Replace(Configuration.StaticFilesLocation, "");
            return $"/Files/{imagePath}";
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