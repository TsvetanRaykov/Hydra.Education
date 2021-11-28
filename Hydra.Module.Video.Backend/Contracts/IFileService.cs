using System.Threading.Tasks;
using Hydra.Module.Video.Backend.Models;

namespace Hydra.Module.Video.Backend.Contracts
{
    public interface IFileService
    {
        Task<string> WriteFileChunkAsync(string fullPath, FileChunk fileChunk);

    }
}