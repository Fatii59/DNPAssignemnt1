namespace Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using RepostitoryContracts;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICommentRepository _commentRepository;

    public PostService(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await _postRepository.GetSingleAsync(id);
    }

    private async Task ValidatePost(string title, string body, int userId)
    {
        if (string.IsNullOrEmpty(title))
            throw new ArgumentException("Title cannot be empty.");
        if (title.Length > 100)
            throw new ArgumentException("Title exceeds the maximum length.");

        if (string.IsNullOrEmpty(body))
            throw new ArgumentException("Body cannot be empty.");
        if (body.Length > 2000)
            throw new ArgumentException("Body exceeds the maximum length.");

        var userExists = await _userRepository.GetSingleAsync(userId) != null;
        if (!userExists)
            throw new ArgumentException("Invalid UserId provided.");
    }

    public async Task<Post> CreatePostAsync(string title, string body, int userId)
    {
        await ValidatePost(title, body, userId);
        var post = new Post(title, body, userId);
        return await _postRepository.AddAsync(post);
    }

    public async Task UpdatePostAsync(Post post)
    {
        var existingPost = await _postRepository.GetSingleAsync(post.Id);
        if (existingPost == null)
            throw new ArgumentException("Post not found.");

        await _postRepository.UpdateAsync(post);
    }

    public async Task DeletePostAsync(int id)
    {
        var post = await _postRepository.GetSingleAsync(id);
        if (post == null)
            throw new ArgumentException($"Post with ID {id} does not exist.");

        await _postRepository.DeleteAsync(id);
    }

    public async Task<List<Post>> GetAllPostsAsync()
    {
        var posts = await _postRepository.GetMany().ToListAsync(); // Asynchronous
        return posts;
    }

    public async Task<List<Post>> GetRecentPostsAsync(int count)
    {
        var posts = await _postRepository.GetMany()
            .OrderByDescending(p => p.CreatedDate) // Asynchronous
            .Take(count)
            .ToListAsync();
        return posts;
    }


}
