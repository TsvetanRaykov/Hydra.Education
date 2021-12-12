using System.Collections.Generic;
using System.Threading.Tasks;
using Hydra.Module.Video.Backend.Models;

namespace Hydra.Module.Video.Backend.Contracts
{
    public interface IVideoService
    {
        Task<string> CreateVideoAsync(VideoRequestDto video, string uploaderId, string url);
        Task<IEnumerable<VideoResponseDto>> GetVideosAsync(int playlistId);
        Task<ClassResponseDto> GetVideoAsync(int id);
    }
}