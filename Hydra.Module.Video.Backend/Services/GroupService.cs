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

        public async Task<string> CreateVideoGroupAsync(string name, string description, string imageUrl, int classId)
        {
            var newGroup = new VideoGroup()
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                VideoClassId = classId
            };

            await _dbContext.VideoGroups.AddAsync(newGroup);

            return await Update(_dbContext);
        }
    }
}