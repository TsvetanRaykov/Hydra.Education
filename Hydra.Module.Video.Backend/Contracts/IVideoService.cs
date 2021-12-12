namespace Hydra.Module.Video.Backend.Contracts
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IVideoService
    {
        Task<string> CreateVideoAsync(VideoRequestDto video, string uploaderId, string fullFilePath);
        Task<IEnumerable<VideoResponseDto>> GetVideosInPlayListsAsync(int[] playlists);
        Task<IEnumerable<VideoResponseDto>> GetVideosByUploader(string uploaderId);
        Task<VideoResponseDto> GetVideoAsync(int id);
    }
}