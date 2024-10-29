namespace BlazorApp1.Services;

using System.Net.Http;
using System.Net.Http.Json;
using DTOs;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient _client;

    public HttpCommentService(HttpClient client)
    {
        _client = client;
    }

    public async Task<CommentDTO> CreateCommentAsync(CreateCommentDTO request)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("api/Comments", request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to add comment");
        }

        // Null-check after deserialization
        var comment = await response.Content.ReadFromJsonAsync<CommentDTO>();
        return comment ?? throw new Exception("Comment creation returned null data");
    }

    public async Task<IEnumerable<CommentDTO>> GetCommentsByPostIdAsync(int postId)
    {
        var comments = await _client.GetFromJsonAsync<IEnumerable<CommentDTO>>($"api/Comments?postId={postId}");
        return comments ?? Enumerable.Empty<CommentDTO>();
    }


    public async Task<CommentDTO> GetCommentByIdAsync(int id)
    {
        var response = await _client.GetAsync($"api/Comments/{id}");
        response.EnsureSuccessStatusCode();

        var comment = await response.Content.ReadFromJsonAsync<CommentDTO>();
        return comment ?? throw new Exception($"Comment with ID {id} not found");
    }

    public async Task UpdateCommentAsync(int id, UpdateCommentDTO request)
    {
        HttpResponseMessage response = await _client.PutAsJsonAsync($"api/Comments/{id}", request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to update comment with ID {id}");
        }
    }

    public async Task DeleteCommentAsync(int id)
    {
        HttpResponseMessage response = await _client.DeleteAsync($"api/Comments/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to delete comment with ID {id}");
        }
    }
}


