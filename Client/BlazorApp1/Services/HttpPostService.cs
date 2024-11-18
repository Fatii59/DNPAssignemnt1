using System.Net.Http.Headers;
using DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorApp1.Services
{
    public class HttpPostService : IPostService
    {
        private readonly HttpClient _client;
        private readonly IJSRuntime _jsRuntime; // Add this

        public HttpPostService(HttpClient client, IJSRuntime jsRuntime)
        {
            _client = client;
            _jsRuntime = jsRuntime; // Assign injected runtime
        }

        public async Task<PostDTO> CreatePostAsync(CreatePostDTO request)
        {
            // Retrieve JWT token from sessionStorage
            var authToken = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken");
            if (!string.IsNullOrEmpty(authToken))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
                Console.WriteLine($"Authorization Header Set: Bearer {authToken}");
            }
            else
            {
                Console.WriteLine("Auth token is missing. Throwing exception.");
                throw new Exception("Authorization token is missing. Please log in.");
            }

            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Post", request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to create post. Error: {error}");
                throw new Exception($"Failed to create post: {error}");
            }

            var post = await response.Content.ReadFromJsonAsync<PostDTO>();
            return post ?? throw new Exception("Post creation returned null data");
        }
        public async Task<IEnumerable<PostDTO>> GetPostsAsync()
        {
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

        public async Task<IEnumerable<PostDTO>> GetRecentPostsAsync(int count)
        {
            var posts = await _client.GetFromJsonAsync<IEnumerable<PostDTO>>($"api/Post/recent?count={count}");
            return posts ?? Array.Empty<PostDTO>();
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
}
