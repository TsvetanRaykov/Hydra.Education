using System.Threading.Tasks;

namespace Hydra.Module.Video.Backend.Contracts
{
    public interface IGroupService
    {
        Task<string> CreateVideoGroupAsync(string name, string description, string imageUrl, int classId);
    }
}