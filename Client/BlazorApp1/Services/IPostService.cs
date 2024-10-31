namespace BlazorApp1.Services;

using DTOs;



public interface IPostService
{
    Task<PostDTO> CreatePostAsync(CreatePostDTO request);
    Task<IEnumerable<PostDTO>> GetPostsAsync();
    Task<PostDTO> GetPostByIdAsync(int id);
    Task<IEnumerable<PostDTO>> GetRecentPostsAsync(int count);
    Task UpdatePostAsync(int id, UpdatePostDTO request);
    Task DeletePostAsync(int id);
}