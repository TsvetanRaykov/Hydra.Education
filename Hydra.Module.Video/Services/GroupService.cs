namespace Hydra.Module.Video.Services
{
    using Contracts;
    using Models;
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
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

        public async Task<VideoGroup> GetGroupAsync(string id)
        {
            var result = await _httpClient.GetAsync($"api/video/groups/{id}");
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadFromJsonAsync<VideoGroup>();
            return responseBody;
        }

        public async Task<bool> SetUsersAsync(int id, string[] userIds)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/video/groups/{id}/users", userIds);
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadFromJsonAsync<bool>();
            return response;
        }

        public async Task<bool> AddPlaylistAsync(int groupId, int playlistId)
        {
            var result = await _httpClient.PostAsync($"api/video/groups/{groupId}/playlists/{playlistId}", null);
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadFromJsonAsync<bool>();
            return response;
        }

        public async Task<bool> DeleteGroupAsync(string id)
        {
            var result = await _httpClient.DeleteAsync($"api/video/groups/{id}");
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadAsStringAsync();
            return Convert.ToBoolean(responseBody);
        }
    }
}