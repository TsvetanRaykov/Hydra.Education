namespace Hydra.Module.Video.Backend.Contracts
{
    using Models;
    using System.Threading.Tasks;

    public interface IFileService
    {
        Task<string> WriteFileChunkAsync(string fullPath, FileChunk fileChunk);
        string DeleteFile(string fullPath);
    }
}