namespace Hydra.Module.Video.Backend.Services
{
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ClassService : ServiceBase, IClassService
    {
        private readonly VideoDbContext _dbContext;

        public ClassService(ILogger<ClassService> logger, VideoDbContext dbContext)
            : base(logger)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateClassAsync(string name, string description, string imageUrl, string trainerId)
        {
            var newClass = new VideoClass
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                TrainerId = trainerId
            };

            await _dbContext.VideoClasses.AddAsync(newClass);

            return await UpdateDbAsync(_dbContext);
        }

        public async Task<ClassResponseDto> GetClassAsync(int id)
        {
            var @class = await _dbContext
                .VideoClasses
                .Include(c => c.VideoGroups)
                .ThenInclude(g => g.Playlists)
                .ThenInclude(p => p.Playlist)
                .ThenInclude(p => p.Videos)
                .ThenInclude(v => v.Video)
                .Include(c => c.VideoGroups)
                .ThenInclude(g => g.Users)
                .FirstAsync(c => c.Id.Equals(id));

            return new ClassResponseDto
            {
                Name = @class.Name,
                ImageUrl = @class.ImageUrl,
                Description = @class.Description,
                Id = @class.Id,
                Groups = @class.VideoGroups.Select(group => new GroupResponseDto
                {
                    Class = new ClassResponseDto
                    {
                        Name = @class.Name,
                        Id = @class.Id,
                        Description = @class.Description,
                        ImageUrl = @class.ImageUrl,
                        TrainerId = @class.TrainerId
                    },
                    Id = group.Id,
                    Name = group.Name,
                    Description = group.Description,
                    ImageUrl = group.ImageUrl,
                    Users = group.Users.Select(u => u.UserId).ToList(),
                    Playlists = group.Playlists.Select(p => new PlaylistResponseDto
                    {
                        Id = p.PlaylistId,
                        Name = p.Playlist.Name,
                        Description = p.Playlist.Description,
                        ImageUrl = p.Playlist.ImageUrl,
                        Videos = p.Playlist.Videos.Select(v => new VideoResponseDto
                        {
                            Id = v.VideoId,
                            Name = v.Video.Name,
                            Description = v.Video.Description,
                            UploadedBy = v.Video.UploadedBy,
                            UploadedOn = v.Video.UploadedOn,
                            Url = v.Video.Url
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
        }

        public async Task<string> UpdateClassAsync(int id, string name, string description, string imageUrl)
        {
            var @class = await _dbContext.FindAsync<VideoClass>(id);

            if (@class == null) return "Class not found.";

            var toUpdate = false;
            if (@class.Name != name)
            {
                @class.Name = name;
                toUpdate = true;
            }
            if (@class.Description != description)
            {
                @class.Description = description;
                toUpdate = true;
            }
            if (@class.ImageUrl != imageUrl)
            {
                @class.ImageUrl = imageUrl;
                toUpdate = true;
            }

            if (toUpdate)
            {
                return await UpdateDbAsync(_dbContext);
            }

            return null;
        }

        public async Task<IEnumerable<ClassResponseDto>> GetClassesAsync(string user)
        {
            var classes = await _dbContext
                .VideoClasses
                .Where(c => c.TrainerId.Equals(user))
                .Include(c => c.VideoGroups).ToListAsync();

            return classes.Select(@class => new ClassResponseDto
            {
                Name = @class.Name,
                ImageUrl = @class.ImageUrl,
                Description = @class.Description,
                Id = @class.Id,
                Groups = @class.VideoGroups.Select(group => new GroupResponseDto
                {
                    Class = new ClassResponseDto()
                    {
                        Name = @class.Name,
                        Id = @class.Id,
                        ImageUrl = @class.ImageUrl,
                        Description = @class.Description,
                        TrainerId = @class.TrainerId
                    },
                    Id = group.Id,
                    Name = group.Name,
                    Description = group.Description,
                    ImageUrl = group.ImageUrl
                }).ToList()
            }).ToList();
        }

    }
}