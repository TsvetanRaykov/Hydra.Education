using System.Threading.Tasks;

namespace Hydra.Module.Video.Contracts
{
    public interface IFileService
    {
        Task<bool> UploadFile<T>(T file) where T : IChunk;
    }
}