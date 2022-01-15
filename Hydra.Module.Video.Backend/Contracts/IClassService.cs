namespace Hydra.Module.Video.Backend.Contracts
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClassService
    {
        Task<string> CreateClassAsync(string name, string description, string imageUrl, string trainerId);
        Task<IEnumerable<ClassResponseDto>> GetClassesAsync(string user);
        Task<ClassResponseDto> GetClassAsync(int id);
        Task<string> UpdateClassAsync(int id, string name, string description, string imageUrl);
        Task<string> DeleteClassAsync(int id);
    }
}