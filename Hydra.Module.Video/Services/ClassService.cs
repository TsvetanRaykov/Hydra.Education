using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Hydra.Module.Video.Models;
using Hydra.Module.Video.Services.Contracts;

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

        public async Task<List<VideoClass>> GetClasses(string user)
        {
            var result = await _httpClient.GetAsync($"api/video/classes/user/{user}");
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadFromJsonAsync<List<VideoClass>>();
            return responseBody;
        }

        public Task<bool> UpdateClassAsync(VideoClass videoClass)
        {
            throw new NotImplementedException();
        }
    }
}