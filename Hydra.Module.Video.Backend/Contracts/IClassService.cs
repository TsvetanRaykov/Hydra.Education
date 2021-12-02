using Hydra.Module.Video.Backend.Models;
using System.Collections.Generic;

namespace Hydra.Module.Video.Backend.Contracts
{
    using System.Threading.Tasks;

    public interface IClassService
    {
        Task<string> CreateClassAsync(string name, string description, string imageUrl, string trainerId);
        Task<IEnumerable<ClassResponseDto>> GetClassesAsync(string user);
        Task<ClassResponseDto> GetClassAsync(int id);
    }
}