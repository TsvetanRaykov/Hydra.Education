namespace Hydra.Module.Video.Backend.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IGroupService
    {
        Task<string> CreateGroupAsync(string name, string description, string imageUrl, int classId);
        Task<GroupResponseDto> GetGroupAsync(int id);
        Task<string> UpdateGroupAsync(int id, string name, string description, string imageUrl);
        Task<string> SetUsersAsync(int groupId, IEnumerable<string> usersIds);
        Task<string> AddPlaylist(int groupId, int playlistId);
        Task<string> RemovePlaylist(int groupId, int playlistId);
        Task<string> DeleteGroupAsync(int id);
    }
}