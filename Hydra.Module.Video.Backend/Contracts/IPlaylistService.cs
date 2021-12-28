namespace Hydra.Module.Video.Backend.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    public interface IPlaylistService
    {
        Task<string> CreatePlaylistAsync(string name, string description, string imageUrl, string trainerId);
        Task<IEnumerable<PlaylistResponseDto>> GetPlayListsAsync(string user);
        Task<PlaylistResponseDto> GetPlaylistAsync(int id);
        Task<string> UpdatePlaylistAsync(int id, string name, string description, string imageUrl);
        Task<string> AddVideo(int id, int videoId);
        Task<string> RemoveVideo(int id, int videoId);
    }
}