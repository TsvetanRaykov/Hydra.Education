using System;
using System.Net.Http.Json;

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

        public GroupService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("authorized");
        }

        public async Task<bool> CreateGroupAsync(VideoGroup videoGroup)
        {
            var result = await _httpClient.PostAsJsonAsync("api/video/groups", videoGroup);
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadAsStringAsync();
            return Convert.ToBoolean(responseBody);
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