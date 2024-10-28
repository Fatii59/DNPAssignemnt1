namespace Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using RepostitoryContracts;


    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _commentRepository.GetSingleAsync(id);
        }

        public async Task<Comment> CreateCommentAsync(string body, int postId, int userId)
        {
            if (string.IsNullOrEmpty(body))
                throw new ArgumentException("Comment body cannot be empty.");

            var comment = new Comment { Body = body, PostId = postId, UserId = userId };
            return await _commentRepository.AddAsync(comment);
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            var existingComment = await _commentRepository.GetSingleAsync(comment.Id);
            if (existingComment == null)
                throw new ArgumentException("Comment not found.");

            await _commentRepository.UpdateAsync(comment);
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _commentRepository.GetSingleAsync(id);
            if (comment == null)
                throw new ArgumentException($"Comment with ID {id} does not exist.");

            await _commentRepository.DeleteAsync(id);
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            var comments = await _commentRepository.GetManyAsync();
            return comments.ToList();
        }
    }

