using System;
using System.Net.Http.Json;
using Hydra.Module.Video.Contracts;

namespace Hydra.Module.Video.Services
{
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

        public async Task<bool> UpdateGroupAsync(VideoGroup videoGroup)
        {
            var result = await _httpClient.PutAsJsonAsync("api/video/groups", videoGroup);
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadAsStringAsync();
            return Convert.ToBoolean(responseBody);
        }

        public Task<List<VideoGroup>> GetUserGroupsAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<VideoGroup>> GetClassGroupsAsync(string classId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<VideoGroup> GetGroupAsync(string id)
        {
            var result = await _httpClient.GetAsync($"api/video/groups/{id}");
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadFromJsonAsync<VideoGroup>();
            return responseBody;
        }

        public Task<bool> DeleteGroupAsync(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}