using Entities;

namespace Services;

public interface ICommentService
{
    Task<Comment?> GetCommentByIdAsync(int id);
    Task<Comment> CreateCommentAsync(string body, int postId, int userId);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(int id);
    Task<List<Comment>> GetAllCommentsAsync();
}