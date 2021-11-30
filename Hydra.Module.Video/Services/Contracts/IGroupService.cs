namespace Hydra.Module.Video.Services.Contracts
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGroupService
    {
        Task<bool> CreateGroupAsync(VideoGroup videoGroup);
        Task<bool> UpdateGroupAsync(VideoGroup videoGroup);
        Task<List<VideoGroup>> GetUserGroupsAsync(string userId);
        Task<List<VideoGroup>> GetClassGroupsAsync(string classId);
        Task<VideoGroup> GetGroupAsync(int id);
        Task<bool> DeleteGroupAsync(int id);
    }
}