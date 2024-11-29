namespace Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using RepostitoryContracts;
using Services.Exceptions;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public CommentService(ICommentRepository commentRepository, IPostRepository postRepository, IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task<Comment?> GetCommentByIdAsync(int id)
    {
        return await _commentRepository.GetSingleAsync(id);
    }

    public async Task<Comment> CreateCommentAsync(string body, int postId, int userId)
    {
        ValidateCommentBody(body);
        await EnsurePostAndUserExist(postId, userId);

        var comment = new Comment(body, postId, userId);
        return await _commentRepository.AddAsync(comment);
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        var existingComment = await _commentRepository.GetSingleAsync(comment.Id);
        if (existingComment == null)
            throw new ResourceNotFoundException("Comment not found.");

        ValidateCommentBody(comment.Body);
        await _commentRepository.UpdateAsync(comment);
    }

    public async Task DeleteCommentAsync(int id)
    {
        var comment = await _commentRepository.GetSingleAsync(id);
        if (comment == null)
            throw new ResourceNotFoundException($"Comment with ID {id} does not exist.");

        await _commentRepository.DeleteAsync(id);
    }

    public async Task<List<Comment>> GetAllCommentsAsync()
    {
        var comments = await _commentRepository.GetManyAsync();
        return comments.ToList();
    }

    private void ValidateCommentBody(string body)
    {
        if (string.IsNullOrEmpty(body))
            throw new ArgumentException("Comment body cannot be empty.");
        if (body.Length > 500)
            throw new ArgumentException("Comment body exceeds the maximum length.");
    }

    private async Task EnsurePostAndUserExist(int postId, int userId)
    {
        var postExists = await _postRepository.GetSingleAsync(postId) != null;
        var userExists = await _userRepository.GetSingleAsync(userId) != null;

        if (!postExists)
            throw new ArgumentException("Invalid PostId provided.");
        if (!userExists)
            throw new ArgumentException("Invalid UserId provided.");
    }
}
