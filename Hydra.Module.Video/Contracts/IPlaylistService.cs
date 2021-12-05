using System.Threading.Tasks;
using Hydra.Module.Video.Models;

namespace Hydra.Module.Video.Contracts
{
    public interface IPlaylistService
    {
        Task<bool> CreatePlaylist(Playlist playlist);
    }
}