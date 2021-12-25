using System.Linq;
using Hydra.Module.Video.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Module.Video.Backend.Services
{
    using Contracts;
    using Data;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class GroupService : ServiceBase, IGroupService
    {
        private readonly VideoDbContext _dbContext;

        public GroupService(ILogger<GroupService> logger, VideoDbContext dbContext)
            : base(logger)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateGroupAsync(string name, string description, string imageUrl, int classId)
        {
            var newGroup = new VideoGroup()
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                VideoClassId = classId
            };

            await _dbContext.VideoGroups.AddAsync(newGroup);

            return await UpdateDbAsync(_dbContext);
        }

        public async Task<GroupResponseDto> GetGroupAsync(int id)
        {
            var group = await _dbContext
                .VideoGroups
                .Include(g => g.VideoClass)
                .Include(g => g.Users)
                .Include(g => g.Playlists)
                .ThenInclude(p => p.Playlist.Videos)
                .FirstAsync(c => c.Id.Equals(id));

            return new GroupResponseDto()
            {
                Name = group.Name,
                ImageUrl = group.ImageUrl,
                Description = group.Description,
                Id = group.Id,
                Playlists = group.Playlists.Select(p => new PlaylistResponseDto
                {
                    Name = p.Playlist.Name,
                    Videos = p.Playlist.Videos.Select(v => new VideoResponseDto
                    {
                        Name = v.Video.Name,
                        Description = v.Video.Description,
                        Id = v.VideoId,
                        UploadedBy = v.Video.UploadedBy,
                        Url = v.Video.Url
                    }).ToList(),
                }).ToList(),
                Users = group.Users.Select(u => u.UserId).ToList(),
                Class = new VideoClass
                {
                    Name = group.VideoClass.Name,
                    Id = group.VideoClass.Id,
                    ImageUrl = group.VideoClass.ImageUrl,
                    TrainerId = group.VideoClass.TrainerId,
                    Description = group.VideoClass.Description
                }
            };
        }

        public async Task<string> UpdateGroupAsync(int id, string name, string description, string imageUrl)
        {
            var group = await _dbContext.FindAsync<VideoGroup>(id);
            var toUpdate = false;
            if (group.Name != name)
            {
                group.Name = name;
                toUpdate = true;
            }
            if (group.Description != description)
            {
                group.Description = description;
                toUpdate = true;
            }
            if (group.ImageUrl != imageUrl)
            {
                group.ImageUrl = imageUrl;
                toUpdate = true;
            }

            if (toUpdate)
            {
                return await UpdateDbAsync(_dbContext);
            }

            return null;
        }

        public async Task<string> SetUsersToGroup(int groupId, string[] usersIds)
        {
            var @group = await _dbContext.VideoGroups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (@group != null)
            {
                @group.Users.Clear();
                @group.Users.AddRange(usersIds.Select(id => new UserToGroup { UserId = id, VideoGroup = @group }));
                return await UpdateDbAsync(_dbContext);
            }

            return "Group not found";
        }

        public Task<string> DeleteGroupAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}