namespace Hydra.Module.Video.Contracts
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IVideoService
    {
        Task<bool> UploadVideoAsync(VideoUploadRequest video);
        Task<Video> GetVideoAsync(int id);
        Task<IEnumerable<Video>> GetOwnedVideosAsync(string ownerId);
        Task<IEnumerable<Video>> GetAllVideosAsync();
        Task<bool> DeleteVideoAsync(string id);
    }

}