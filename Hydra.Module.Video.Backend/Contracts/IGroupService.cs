﻿using System.Threading.Tasks;
using Hydra.Module.Video.Backend.Models;

namespace Hydra.Module.Video.Backend.Contracts
{
    public interface IGroupService
    {
        Task<string> CreateGroupAsync(string name, string description, string imageUrl, int classId);
        Task<GroupResponseDto> GetGroupAsync(int id);
        Task<string> UpdateGroupAsync(int id, string name, string description, string imageUrl);
        Task<string> SetUsersToGroup(int groupId, string[] usersIds);
        Task<string> DeleteGroupAsync(int id);
    }
}