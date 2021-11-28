using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hydra.Module.Video.Backend.Services
{
    using Contracts;
    using Data;

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