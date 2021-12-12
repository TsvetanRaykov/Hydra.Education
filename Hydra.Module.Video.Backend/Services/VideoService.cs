namespace Hydra.Module.Video.Backend.Services
{
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    public class VideoService : ServiceBase, IVideoService
    {
        private readonly VideoDbContext _dbContext;

        public VideoService(ILogger<VideoService> logger, VideoDbContext dbContext) : base(logger)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateVideoAsync(VideoRequestDto video, string uploaderId, string fullFilePath)
        {
            var videoUrl = $"Files/{Path.GetFileName(fullFilePath)}";

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
                Url = videoUrl,
                Description = video.Description
            };

            await _dbContext.Videos.AddAsync(newVideo);

            return await UpdateDbAsync(_dbContext);
        }

        public async Task<IEnumerable<VideoResponseDto>> GetVideosInPlayListsAsync(int[] playlists)
        {
            var videos = await _dbContext
                .Playlists.Where(p => playlists.Contains(p.Id))
                .Select(p => p.Videos)
                .SelectMany(v2P => v2P.Select(v => v.Video))
                .Select(v => new VideoResponseDto
                {
                    Name = v.Name,
                    Description = v.Description,
                    Id = v.Id,
                    UploadedBy = v.UploadedBy,
                    UploadedOn = v.UploadedOn,
                    Url = v.Url,
                    PlayLists = v.Playlists.Select(v2P => new PlaylistResponseDto
                    {
                        Id = v2P.Playlist.Id,
                        Description = v2P.Playlist.Description,
                        ImageUrl = v2P.Playlist.ImageUrl,
                        Name = v2P.Playlist.Name,
                    }).ToList(),
                })
                .ToArrayAsync();

            return videos;
        }

        public async Task<IEnumerable<VideoResponseDto>> GetVideosByUploader(string uploaderId)
        {
            var videos = await _dbContext
                .Videos.Where(v => v.UploadedBy.Equals(uploaderId))
                .Select(v => new VideoResponseDto
                {
                    Name = v.Name,
                    Description = v.Description,
                    Id = v.Id,
                    UploadedBy = v.UploadedBy,
                    UploadedOn = v.UploadedOn,
                    Url = v.Url,
                    PlayLists = v.Playlists.Select(v2P => new PlaylistResponseDto
                    {
                        Id = v2P.Playlist.Id,
                        Description = v2P.Playlist.Description,
                        ImageUrl = v2P.Playlist.ImageUrl,
                        Name = v2P.Playlist.Name,
                    }).ToList(),
                })
                .ToArrayAsync();

            return videos;
        }

        public async Task<VideoResponseDto> GetVideoAsync(int id)
        {
            var video = await _dbContext.Videos
                .Include(v => v.Playlists)
                .ThenInclude(p => p.Playlist)
                .ThenInclude(p => p.VideoGroups)
                .ThenInclude(g => g.VideoGroup)

                .FirstOrDefaultAsync(v => v.Id == id);
            if (video == null) return null;

            return new VideoResponseDto
            {
                Id = video.Id,
                Name = video.Name,
                Description = video.Description,
                PlayLists = video.Playlists.Select(p => new PlaylistResponseDto
                {
                    Id = p.PlaylistId,
                    Description = p.Playlist.Description,
                    Name = p.Playlist.Name,
                    ImageUrl = p.Playlist.ImageUrl,
                }).ToList(),
                UploadedBy = video.UploadedBy,
                UploadedOn = video.UploadedOn,
                Url = video.Url
            };
        }

        private Image GenerateThumbnail(Uri imageUri, int width = 100, int height = 100)
        {
            try
            {
                using var fileStream = File.OpenRead(imageUri.AbsolutePath);
                var image = Image.FromStream(fileStream);
                var thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);

                return thumb;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, e);
                return null;
            }
        }
    }
}