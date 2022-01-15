namespace Hydra.Module.Video.Services;

using Contracts;
using Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class StudentsService : IStudentsService
{
    private readonly HttpClient _httpClient;

    public StudentsService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("authorized");
    }

    public async Task<StudentDto[]> GetStudentsAsync()
    {
        var result = await _httpClient.GetAsync($"/User/students");
        result.EnsureSuccessStatusCode();
        var responseBody = await result.Content.ReadFromJsonAsync<StudentDto[]>();
        return responseBody;
    }

    public async Task<VideoGroup[]> GetStudentGroups(string studentId)
    {
        var result = await _httpClient.GetAsync($"api/video/students/{studentId}/groups");
        result.EnsureSuccessStatusCode();
        var responseBody = await result.Content.ReadFromJsonAsync<VideoGroup[]>();
        return responseBody;
    }
}