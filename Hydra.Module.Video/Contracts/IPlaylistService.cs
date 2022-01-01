namespace Hydra.Module.Video.Contracts
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IPlaylistService
    {
        Task<bool> CreatePlaylistAsync(VideoPlaylist playlist);
        Task<bool> UpdatePlaylistAsync(VideoPlaylist videoPlaylist);
        Task<List<VideoPlaylist>> GetPlayListsAsync(string userId);
        Task<VideoPlaylist> GetPlaylistAsync(string id);
        Task<bool> DeletePlaylistAsync(string id);
        Task<bool> AddVideoAsync(string playlistId, string videoId);
        Task<bool> RemoveVideoAsync(string playlistId, string videoId);
    }
}