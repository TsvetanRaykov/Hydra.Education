using System.Collections.Generic;
using System.Threading.Tasks;
using Hydra.Module.Video.Models;

namespace Hydra.Module.Video.Contracts
{
    public interface IGroupService
    {
        Task<bool> CreateGroupAsync(VideoGroup videoGroup);
        Task<bool> UpdateGroupAsync(VideoGroup videoGroup);
        Task<List<VideoGroup>> GetUserGroupsAsync(string userId);
        Task<List<VideoGroup>> GetClassGroupsAsync(string classId);
        Task<VideoGroup> GetGroupAsync(string id);
        Task<bool> DeleteGroupAsync(string id);
    }
}