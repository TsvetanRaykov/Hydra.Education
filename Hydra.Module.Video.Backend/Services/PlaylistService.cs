using System.Threading.Tasks;
using Hydra.Module.Video.Backend.Contracts;
using Hydra.Module.Video.Backend.Data;
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

    }
}