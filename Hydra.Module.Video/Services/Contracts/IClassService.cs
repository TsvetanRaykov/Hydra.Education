using System.Threading.Tasks;
using Hydra.Module.Video.Models;

namespace Hydra.Module.Video.Services.Contracts
{
    public interface IClassService
    {
        Task<bool> CreateClassAsync(VideoClass videoClass);
    }
}