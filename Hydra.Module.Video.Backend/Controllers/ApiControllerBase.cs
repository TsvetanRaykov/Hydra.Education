using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Hydra.Module.Video.Backend.Contracts;
using Hydra.Module.Video.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Module.Video.Backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/video/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected static string BuildImagePath(string className)
        {
            var imageName = Convert.ToBase64String(Encoding.UTF8.GetBytes(className));
            
            // $"{name}-{DateTime.Now.Ticks}.png";
            var imagePath = $"Files/{imageName}.png";
            return imagePath;
        }


        protected async Task<string> SaveImage(IFileService fileService, string imagePath, byte[] imageData)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, imagePath);

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