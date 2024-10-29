
namespace BlazorApp1.Services;

using System.Net.Http;
using System.Net.Http.Json;
using DTOs;

public class HttpUserService : IUserService
{
    private readonly HttpClient _client;

    public HttpUserService(HttpClient client)
    {
        _client = client;
    }

    public async Task<UserDTO> AddUserAsync(CreateUserDTO request)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("api/Users", request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to add user");
        }

        // Check for null after deserialization
        var user = await response.Content.ReadFromJsonAsync<UserDTO>();
        return user ?? throw new Exception("User creation returned null data");
    }

    public async Task<UserDTO> GetUserByIdAsync(int id)
    {
        var response = await _client.GetAsync($"api/Users/{id}");
        response.EnsureSuccessStatusCode();

        var user = await response.Content.ReadFromJsonAsync<UserDTO>();
        return user ?? throw new Exception($"User with ID {id} not found");
    }

    public async Task<IEnumerable<UserDTO>> GetUsersAsync(string? userName = null)
    {
        var query = string.IsNullOrEmpty(userName) ? "" : $"?userName={userName}";

        // Use ?? to ensure non-null return value
        var users = await _client.GetFromJsonAsync<IEnumerable<UserDTO>>($"api/Users{query}");
        return users ?? Array.Empty<UserDTO>();
    }
}
