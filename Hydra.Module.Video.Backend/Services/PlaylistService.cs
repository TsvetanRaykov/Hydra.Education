using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Module.Video.Backend.Contracts;
using Hydra.Module.Video.Backend.Data;
using Hydra.Module.Video.Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hydra.Module.Video.Backend.Services
{
    public class PlaylistService : ServiceBase, IPlaylistService
    {
        private readonly VideoDbContext _dbContext;
        public PlaylistService(ILogger<PlaylistService> logger, VideoDbContext dbContext) : base(logger)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreatePlaylistAsync(string name, string description, string imageUrl, string trainerId)
        {
            var newPlaylist = new Playlist
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                TrainerId = trainerId
            };

            await _dbContext.Playlists.AddAsync(newPlaylist);

            return await UpdateDbAsync(_dbContext);
        }

        public async Task<IEnumerable<PlaylistResponseDto>> GetPlayListsAsync(string user)
        {
            var playlist = await _dbContext
                .Playlists
                .Where(c => c.TrainerId.Equals(user))
                .Include(c => c.VideoGroups)
                .ThenInclude(g => g.Group)
                .Include(p => p.Videos)
                .ThenInclude(p => p.Video)
                .ToListAsync();

            return playlist.Select(p => new PlaylistResponseDto
            {
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Description = p.Description,
                Id = p.Id,
                Videos = p.Videos.Select(v => new VideoResponseDto()
                {
                    Id = v.VideoId,
                    Name = v.Video.Name,
                    Description = v.Video.Description,
                    UploadedOn = v.Video.UploadedOn
                }).ToList(),
                VideoGroups = p.VideoGroups.Select(g => new GroupResponseDto
                {
                    Name = g.Group.Name,
                    Description = g.Group.Description,
                    Id = g.GroupId,
                    ImageUrl = g.Group.ImageUrl,
                    Class = g.Group.VideoClass
                }).ToList()

            }).ToList();
        }

        public async Task<PlaylistResponseDto> GetPlaylistAsync(int id)
        {
            var playlist = await _dbContext
                .Playlists
                .Include(c => c.VideoGroups)
                .ThenInclude(g => g.Group)
                .ThenInclude(g => g.Users)
                .Include(p => p.Videos)
                .ThenInclude(g => g.Video)
                .FirstAsync(p => p.Id.Equals(id));

            return new PlaylistResponseDto
            {
                Id = playlist.Id,
                Name = playlist.Name,
                Description = playlist.Description,
                ImageUrl = playlist.ImageUrl,
                VideoGroups = playlist.VideoGroups.Select(g => new GroupResponseDto
                {
                    Id = g.GroupId,
                    Name = g.Group.Name,
                    ImageUrl = g.Group.ImageUrl,
                    Description = g.Group.Description,
                    Class = g.Group.VideoClass,
                    Users = g.Group.Users.Select(u => u.UserId).ToList(),
                }).ToList(),
                Videos = playlist.Videos.Select(v => new VideoResponseDto
                {
                    Id = v.Video.Id,
                    Description = v.Video.Description,
                    Name = v.Video.Name,
                    UploadedBy = v.Video.UploadedBy,
                    Url = v.Video.Url,
                    UploadedOn = v.Video.UploadedOn
                }).ToList()
            };
        }

        public async Task<string> UpdatePlaylistAsync(int id, string name, string description, string imageUrl)
        {
            var playlist = await _dbContext.FindAsync<Playlist>(id);

            if (playlist == null) return "Playlist not found.";

            var toUpdate = false;
            if (playlist.Name != name)
            {
                playlist.Name = name;
                toUpdate = true;
            }
            if (playlist.Description != description)
            {
                playlist.Description = description;
                toUpdate = true;
            }
            if (playlist.ImageUrl != imageUrl)
            {
                playlist.ImageUrl = imageUrl;
                toUpdate = true;
            }

            if (toUpdate)
            {
                return await UpdateDbAsync(_dbContext);
            }

            return null;
        }

        public async Task<string> AddVideo(int id, int videoId)
        {
            var playlist = await _dbContext.Playlists
                .Include(p => p.Videos)
                .ThenInclude(v => v.Video)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (playlist == null) return "Playlist not found.";

            var exist = playlist.Videos.FirstOrDefault(v => v.Video.Id == videoId);
            if (exist != null) return null;

            playlist.Videos.Add(new VideoToPlaylist
            {
                VideoId = videoId
            });

            return await UpdateDbAsync(_dbContext);
        }

        public async Task<string> RemoveVideo(int id, int videoId)
        {
            var playlist = await _dbContext.Playlists
                .Include(p => p.Videos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null) return "Playlist not found.";
            var video = playlist.Videos.FirstOrDefault(v => v.Video.Id == videoId);
            if (video == null) return null;

            playlist.Videos.Remove(video);

            return await UpdateDbAsync(_dbContext);
        }
    }
}