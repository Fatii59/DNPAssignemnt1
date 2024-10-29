namespace BlazorApp1.Services;

using System.Net.Http;
using System.Net.Http.Json;
using DTOs;


public class HttpPostService : IPostService
{
    private readonly HttpClient _client;

    public HttpPostService(HttpClient client)
    {
        _client = client;
    }

    public async Task<PostDTO> CreatePostAsync(CreatePostDTO request)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("api/Post", request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to create post");
        }

        // Null-check after deserialization
        var post = await response.Content.ReadFromJsonAsync<PostDTO>();
        return post ?? throw new Exception("Post creation returned null data");
    }

    public async Task<IEnumerable<PostDTO>> GetPostsAsync()
    {
        // Null-coalescing to ensure a non-null return value
        var posts = await _client.GetFromJsonAsync<IEnumerable<PostDTO>>("api/Post");
        return posts ?? Array.Empty<PostDTO>();
    }

    public async Task<PostDTO> GetPostByIdAsync(int id)
    {
        var response = await _client.GetAsync($"api/Post/{id}");
        response.EnsureSuccessStatusCode();

        var post = await response.Content.ReadFromJsonAsync<PostDTO>();
        return post ?? throw new Exception($"Post with ID {id} not found");
    }

    public async Task UpdatePostAsync(int id, UpdatePostDTO request)
    {
        HttpResponseMessage response = await _client.PutAsJsonAsync($"api/Post/{id}", request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to update post with ID {id}");
        }
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage response = await _client.DeleteAsync($"api/Post/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to delete post with ID {id}");
        }
    }
}

