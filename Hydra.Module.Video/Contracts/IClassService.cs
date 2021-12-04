using System.Collections.Generic;
using System.Threading.Tasks;
using Hydra.Module.Video.Models;

namespace Hydra.Module.Video.Contracts
{
    public interface IClassService
    {
        Task<bool> CreateClassAsync(VideoClass videoClass);
        Task<bool> UpdateClassAsync(VideoClass videoClass);
        Task<List<VideoClass>> GetClassesAsync(string user);
        Task<VideoClass> GetClassAsync(string id);

    }
}