using Hydra.Module.Video.Models;
using System.Threading.Tasks;

namespace Hydra.Module.Video.Contracts
{
    public interface IGroupService
    {
        Task<bool> CreateGroupAsync(VideoGroup videoGroup);
        Task<bool> UpdateGroupAsync(VideoGroup videoGroup);
        Task<VideoGroup> GetGroupAsync(string id);
        Task<bool> SetUsersAsync(int id, string[] userIds);
        Task<bool> AddPlaylistAsync(int groupId, int playlistId);
        Task<bool> DeleteGroupAsync(string id);
    }
}