using System.Threading.Tasks;
using Hydra.Module.Video.Models;

namespace Hydra.Module.Video.Contracts
{
    public interface IFileService
    {
        Task<bool> UploadFileChunk(FileChunk fileChunk);
    }
}