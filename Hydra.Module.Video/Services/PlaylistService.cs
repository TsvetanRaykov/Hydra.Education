namespace Hydra.Module.Video.Services
{
    using Contracts;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public class PlaylistService : IPlaylistService
    {
        private readonly HttpClient _httpClient;

        public PlaylistService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("authorized");
        }

        public async Task<bool> CreatePlaylistAsync(VideoPlaylist playlist)
        {
            var result = await _httpClient.PostAsJsonAsync("api/video/playlists", playlist);
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadAsStringAsync();
            return Convert.ToBoolean(responseBody);
        }

        public async Task<bool> UpdatePlaylistAsync(VideoPlaylist videoPlaylist)
        {
            var result = await _httpClient.PutAsJsonAsync("api/video/playlists", videoPlaylist);
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadAsStringAsync();
            return Convert.ToBoolean(responseBody);
        }

        public async Task<List<VideoPlaylist>> GetPlayListsAsync(string userId)
        {
            var result = await _httpClient.GetAsync($"api/video/playlists/user/{userId}");
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadFromJsonAsync<List<VideoPlaylist>>();
            return responseBody;
        }

        public async Task<VideoPlaylist> GetPlaylistAsync(string id)
        {
            var result = await _httpClient.GetAsync($"api/video/playlists/{id}");
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadFromJsonAsync<VideoPlaylist>();
            return responseBody;
        }

        public async Task<bool> DeletePlaylistAsync(string id)
        {
            var result = await _httpClient.DeleteAsync($"api/video/playlists/{id}");
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadAsStringAsync();
            return Convert.ToBoolean(responseBody);
        }

        public async Task<bool> AddVideoAsync(string playlistId, string videoId)
        {
            var result = await _httpClient.PostAsync($"api/video/playlists/{playlistId}/video/{videoId}", null);
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadAsStringAsync();
            return Convert.ToBoolean(responseBody);
        }

        public async Task<bool> RemoveVideoAsync(string playlistId, string videoId)
        {
            var result = await _httpClient.DeleteAsync($"api/video/playlists/{playlistId}/video/{videoId}");
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadAsStringAsync();
            return Convert.ToBoolean(responseBody);
        }
    }
}