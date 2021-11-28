using System.Threading.Tasks;

namespace Hydra.Module.Video.Backend.Contracts
{
    public interface IClassService
    {
        Task<string> CreateClass(string name, string description, string imageUrl, string trainerId);
    }
}