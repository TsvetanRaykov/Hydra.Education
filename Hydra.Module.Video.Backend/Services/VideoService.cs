using System;

namespace Hydra.Module.Video.Backend.Services
{
    using Contracts;
    using Data;
    using Microsoft.Extensions.Logging;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class VideoService : ServiceBase, IVideoService
    {
        private readonly VideoDbContext _dbContext;

        public VideoService(ILogger<VideoService> logger, VideoDbContext dbContext) : base(logger)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateVideoAsync(VideoRequestDto video, string uploaderId, string url)
        {
            var newVideo = new Video()
            {
                Name = video.Name,
                Playlists = _dbContext.Playlists.Where(p => video.Playlists.Contains(p.Id)).Select(p => new VideoToPlaylist
                {
                    Playlist = p,
                    PlaylistId = p.Id
                }).ToList(),
                UploadedOn = DateTime.UtcNow,
                UploadedBy = uploaderId,
                Url = url,
                Description = video.Description
            };

            await _dbContext.Videos.AddAsync(newVideo);

            return await UpdateDbAsync(_dbContext);
        }

        public Task<IEnumerable<VideoResponseDto>> GetVideosAsync(int playlistId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ClassResponseDto> GetVideoAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}