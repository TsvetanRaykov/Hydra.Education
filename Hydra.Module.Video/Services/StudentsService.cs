using System;
using Hydra.Module.Video.Contracts;
using Hydra.Module.Video.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Hydra.Module.Video.Services;

public class StudentsService : IStudentsService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    public StudentsService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _httpClient = httpClientFactory.CreateClient("authorized");
        _httpClient.BaseAddress = new Uri(_navigationManager.BaseUri);
    }
    public async Task<StudentDto[]> GetStudentsAsync()
    {
        var result = await _httpClient.GetAsync($"/User/students");
        result.EnsureSuccessStatusCode();
        var responseBody = await result.Content.ReadFromJsonAsync<StudentDto[]>();
        return responseBody;

    }

    public Task<bool> AddStudentsToGroup(string[] studentIds, string groupId)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> RemoveStudentsFromGroup(string[] studentIds, string groupId)
    {
        throw new System.NotImplementedException();
    }
}