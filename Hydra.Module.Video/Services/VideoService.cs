namespace Hydra.Module.Video.Services
{
    using System;
    using Microsoft.Extensions.Logging;
    using System.Net.Http;
    using Contracts;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Net.Http.Json;

    public class VideoService : IVideoService
    {
        private readonly ILogger<VideoService> _logger;
        private readonly HttpClient _httpClient;
        public VideoService(IHttpClientFactory clientFactory, ILogger<VideoService> logger)
        {
            _logger = logger;
            _httpClient = clientFactory.CreateClient("authorized");
        }

        public async Task<bool> UploadVideoAsync(VideoUploadRequest video)
        {
            var result = await _httpClient.PostAsJsonAsync("api/video/videos", video);
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadAsStringAsync();
            return Convert.ToBoolean(responseBody);
        }

        public async Task<IEnumerable<Video>> GetVideosInPlayListsAsync(int[] playlists)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/video/videos/in", playlists);
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadFromJsonAsync<List<Video>>();
            return responseBody;
        }

        public async Task<IEnumerable<Video>> GetOwnedVideosAsync(string ownerId)
        {
            var result = await _httpClient.GetAsync($"api/video/videos/owner/{ownerId}");
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadFromJsonAsync<List<Video>>();
            return responseBody;
        }

        public async Task<Video> GetVideoAsync(int id)
        {
            var result = await _httpClient.GetAsync($"api/video/videos/{id}");
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadFromJsonAsync<Video>();
            return responseBody;
        }
    }
}