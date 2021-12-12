namespace Hydra.Module.Video.Contracts
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IVideoService
    {
        Task<bool> UploadVideoAsync(VideoUploadRequest video);
        Task<IEnumerable<Video>> GetVideosInPlayListsAsync(int[] playlists);
        Task<IEnumerable<Video>> GetOwnedVideosAsync(string ownerId);
        Task<Video> GetVideoAsync(int id);
    }

}