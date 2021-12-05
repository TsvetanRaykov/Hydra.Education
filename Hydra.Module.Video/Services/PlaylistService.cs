using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Hydra.Module.Video.Contracts;
using Hydra.Module.Video.Models;

namespace Hydra.Module.Video.Services
{
    public class PlaylistService : IPlaylistService
    {
        private HttpClient _httpClient;

        public PlaylistService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("authorized");
        }

        public async Task<bool> CreatePlaylist(Playlist playlist)
        {
            var result = await _httpClient.PostAsJsonAsync("api/video/playlists", playlist);
            result.EnsureSuccessStatusCode();
            var responseBody = await result.Content.ReadAsStringAsync();
            return Convert.ToBoolean(responseBody);
        }
    }
}