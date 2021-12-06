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
                .FirstAsync(c => c.Id.Equals(id));

            return new ClassResponseDto
            {
                Name = @class.Name,
                ImageUrl = @class.ImageUrl,
                Description = @class.Description,
                Id = @class.Id,
                Groups = @class.VideoGroups.Select(group => new GroupResponseDto
                {
                    Id = group.Id,
                    Name = group.Name,
                    Description = group.Description,
                    ImageUrl = group.ImageUrl
                }).ToList()
            };
        }

        public async Task<string> UpdateClassAsync(int id, string name, string description, string imageUrl)
        {
            var @class = await _dbContext.FindAsync<VideoClass>(id);
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
                    Id = group.Id,
                    Name = group.Name,
                    Description = group.Description,
                    ImageUrl = group.ImageUrl
                }).ToList()
            }).ToList();
        }

    }
}