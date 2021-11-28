using System;
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
            var result = await _httpClient.PostAsJsonAsync("api/video/class", videoClass);
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadAsStringAsync();
            return Convert.ToBoolean(responseBody);
        }
    }
}