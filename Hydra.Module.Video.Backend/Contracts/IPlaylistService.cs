using System.Threading.Tasks;

namespace Hydra.Module.Video.Backend.Contracts
{
    public interface IPlaylistService
    {
        Task<string> CreatePlaylistAsync(string name, string description, string imageUrl, string trainerId);
    }
}