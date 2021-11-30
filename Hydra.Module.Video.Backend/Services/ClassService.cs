using System.Linq;
using System.Threading.Tasks;
using Hydra.Module.Video.Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hydra.Module.Video.Backend.Services
{
    using Contracts;
    using Data;
    using System.Collections.Generic;

    public class ClassService : IClassService
    {
        private readonly ILogger<ClassService> _logger;

        private readonly VideoDbContext _dbContext;

        public ClassService(ILogger<ClassService> logger, VideoDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<string> CreateClass(string name, string description, string imageUrl, string trainerId)
        {
            var newClass = new VideoClass
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                TrainerId = trainerId
            };

            await _dbContext.VideoClasses.AddAsync(newClass);

            return await Update(_dbContext);
        }

        public async Task<ClassResponseDto> GetClassAsync(int id)
        {
            var cls = await _dbContext
                .VideoClasses
                .Include(c => c.VideoGroups)
                .FirstAsync(c => c.Id.Equals(id));

            return new ClassResponseDto
            {
                Name = cls.Name,
                ImageUrl = cls.ImageUrl,
                Description = cls.Description,
                Id = cls.Id
            };
        }

        public async Task<IEnumerable<ClassResponseDto>> GetClassesAsync(string user)
        {
            var classes = await _dbContext
                .VideoClasses
                .Where(c => c.TrainerId.Equals(user))
                .Include(c => c.VideoGroups).ToListAsync();

            return classes.Select(c => new ClassResponseDto
            {
                Name = c.Name,
                ImageUrl = c.ImageUrl,
                Description = c.Description,
                Id = c.Id
            }).ToList();
        }



        private async Task<string> Update(DbContext db)
        {
            try
            {
                await db.SaveChangesAsync();
                return null;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(nameof(Update), ex);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return "Database operation failed.";
            }
        }
    }
}