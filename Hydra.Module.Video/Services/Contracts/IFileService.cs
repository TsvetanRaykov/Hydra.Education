using Hydra.Module.Video.Models;

namespace Hydra.Module.Video.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IFileService
    {
        Task<bool> UploadFileChunk(FileChunk fileChunk);
    }
}