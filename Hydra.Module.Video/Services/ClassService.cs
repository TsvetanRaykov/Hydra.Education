using Hydra.Module.Video.Contracts;
using Hydra.Module.Video.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hydra.Module.Video.Services
{
    public class ClassService : IClassService
    {
        private readonly HttpClient _httpClient;
        public ClassService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("authorized");
        }

        public async Task<bool> CreateClassAsync(VideoClass videoClass)
        {
            var result = await _httpClient.PostAsJsonAsync("api/video/classes", videoClass);
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadAsStringAsync();
            return Convert.ToBoolean(responseBody);
        }

        public async Task<VideoClass> GetClassAsync(string id)
        {
            var result = await _httpClient.GetAsync($"api/video/classes/{id}");
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadFromJsonAsync<VideoClass>();
            return responseBody;
        }

        public async Task<List<VideoClass>> GetClassesAsync(string user)
        {
            var result = await _httpClient.GetAsync($"api/video/classes/user/{user}");
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadFromJsonAsync<List<VideoClass>>();
            return responseBody;
        }

        public async Task<bool> UpdateClassAsync(VideoClass videoClass)
        {
            var result = await _httpClient.PutAsJsonAsync("api/video/classes", videoClass);
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadAsStringAsync();
            return Convert.ToBoolean(responseBody);
        }
    }
}