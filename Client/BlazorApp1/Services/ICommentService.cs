namespace BlazorApp1.Services;

using DTOs;

public interface ICommentService
{
    Task<CommentDTO> CreateCommentAsync(CreateCommentDTO request);
    Task<IEnumerable<CommentDTO>> GetCommentsByPostIdAsync(int postId);
    Task<CommentDTO> GetCommentByIdAsync(int id);
    Task UpdateCommentAsync(int id, UpdateCommentDTO request);
    Task DeleteCommentAsync(int id);
}