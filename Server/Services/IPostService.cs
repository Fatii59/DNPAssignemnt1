using Entities;

namespace Services;

public interface IPostService
{
    Task<Post?> GetPostByIdAsync(int id);
    Task<Post> CreatePostAsync(string title, string body, int userId);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(int id);
    Task<List<Post>> GetAllPostsAsync();
    Task<List<Post>> GetRecentPostsAsync(int count); // New method to get recent posts
}