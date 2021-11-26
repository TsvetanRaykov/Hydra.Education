using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hydra.Module.Video.Backend.Services
{
    using Contracts;
    using Data;

    public class ClassService : IClassService
    {
        private readonly ModuleVideoConfiguration _configuration;
        private readonly ILogger<ClassService> _logger;

        public ClassService(ModuleVideoConfiguration configuration, ILogger<ClassService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> CreateClass(string name, string description)
        {
            var newClass = new VideoClass
            {
                Name = name,
                Description = description
            };

            await using var db = new VideoDbContext(null);

            await db.VideoClasses.AddAsync(newClass);

            return await Update(db);
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
                return "Database operation failed.";
            }
        }
    }
}