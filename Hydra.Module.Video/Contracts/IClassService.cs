namespace Hydra.Module.Video.Contracts
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClassService
    {
        Task<bool> CreateClassAsync(VideoClass videoClass);
        Task<bool> UpdateClassAsync(VideoClass videoClass);
        Task<List<VideoClass>> GetClassesAsync(string user);
        Task<VideoClass> GetClassAsync(string id);
        Task<bool> DeleteClassAsync(string id);
    }
}