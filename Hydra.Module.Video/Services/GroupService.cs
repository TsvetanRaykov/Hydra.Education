namespace Hydra.Module.Video.Services
{
    using Contracts;
    using Models;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class GroupService : IGroupService
    {
        private readonly HttpClient _httpClient;

        public GroupService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<bool> CreateGroupAsync(VideoGroup videoClass)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateGroupAsync(VideoGroup videoClass)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<VideoGroup>> GetUserGroupsAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<VideoGroup>> GetClassGroupsAsync(string classId)
        {
            throw new System.NotImplementedException();
        }

        public Task<VideoGroup> GetGroupAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteGroupAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}