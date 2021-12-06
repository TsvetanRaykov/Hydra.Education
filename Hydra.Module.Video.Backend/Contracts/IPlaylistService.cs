using System.Collections.Generic;
using System.Threading.Tasks;
using Hydra.Module.Video.Backend.Models;

namespace Hydra.Module.Video.Backend.Contracts
{
    public interface IPlaylistService
    {
        Task<string> CreatePlaylistAsync(string name, string description, string imageUrl, string trainerId);
        Task<IEnumerable<PlaylistResponseDto>> GetPlayListsAsync(string user);
        Task<PlaylistResponseDto> GetPlaylistAsync(int id);
        Task<string> UpdatePlaylistAsync(int id, string name, string description, string imageUrl);
    }
}