namespace Hydra.Module.Video.Backend.Services;

using Contracts;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq;
using System.Threading.Tasks;

public class StudentService : ServiceBase, IStudentService
{
    private readonly VideoDbContext _dbContext;

    public StudentService(ILogger<StudentService> logger, VideoDbContext dbContext)
        : base(logger)
    {
        _dbContext = dbContext;
    }
    public async Task<GroupResponseDto[]> GetStudentGroups(string studentId)
    {
        var groups = await _dbContext
            .VideoGroups
            .Where(g => g.Users.Any(u => u.UserId == studentId))
            .Include(g => g.VideoClass)
            .Include(g => g.Users)
            .Include(g => g.Playlists)
            .ThenInclude(p => p.Playlist.Videos)
            .ThenInclude(v => v.Video)
            .ToArrayAsync();

        return groups.Select(group => new GroupResponseDto()
        {
            Name = group.Name,
            ImageUrl = group.ImageUrl,
            Description = group.Description,
            Id = group.Id,
            Playlists = group.Playlists.Select(p => new PlaylistResponseDto
            {
                Id = p.Playlist.Id,
                Name = p.Playlist.Name,
                Description = p.Playlist.Description,
                ImageUrl = p.Playlist.ImageUrl,
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
            Class = new ClassResponseDto()
            {
                Name = group.VideoClass.Name,
                Id = group.VideoClass.Id,
                ImageUrl = group.VideoClass.ImageUrl,
                TrainerId = group.VideoClass.TrainerId,
                Description = group.VideoClass.Description
            }
        }).ToArray();
    }
}