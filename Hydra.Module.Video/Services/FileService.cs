using Hydra.Module.Video.Models;

namespace Hydra.Module.Video.Services
{
    using Contracts;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly HttpClient _httpClient;

        public FileService(ILogger<FileService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("authorized");
        }

        public async Task<bool> UploadFileChunk(FileChunk fileChunk)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("api/video/file", fileChunk);
                result.EnsureSuccessStatusCode();
                var responseBody = await result.Content.ReadAsStringAsync();
                return Convert.ToBoolean(responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }
    }
}