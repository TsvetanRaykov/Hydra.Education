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
                .Include(p => p.Videos)
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
                    Name = g.VideoGroup.Name,
                    Description = g.VideoGroup.Description,
                    Id = g.GroupId,
                    ImageUrl = g.VideoGroup.ImageUrl,
                    Class = g.VideoGroup.VideoClass
                }).ToList()

            }).ToList();
        }

        public async Task<PlaylistResponseDto> GetPlaylistAsync(int id)
        {
            var playlist = await _dbContext
                .Playlists
                .Include(c => c.VideoGroups)
                .Include(p => p.Videos)
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
                    Name = g.VideoGroup.Name,
                    ImageUrl = g.VideoGroup.ImageUrl,
                    Description = g.VideoGroup.Description,
                    Class = g.VideoGroup.VideoClass,
                    Users = g.VideoGroup.Users.Select(u => u.UserId).ToList(),
                }).ToList()
            };
        }

        public async Task<string> UpdatePlaylistAsync(int id, string name, string description, string imageUrl)
        {
            var playlist = await _dbContext.FindAsync<Playlist>(id);
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
    }
}