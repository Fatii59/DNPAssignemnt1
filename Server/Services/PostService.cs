

using Entities;
using RepostitoryContracts;

namespace Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly  ICommentRepository _commentRepository;

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
        var post = new Post { Title = title, Body = body, UserId = userId };
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
        var posts = await _postRepository.GetMany();
        return posts.ToList();
    }
    
    public async Task<List<Post>> GetRecentPostsAsync(int count)
    {
        // Fetch all posts from the repository
        var posts = await _postRepository.GetMany();

        // Fetch all users to avoid repetitive calls, mapping them by ID
        var users = _userRepository.GetMany().ToDictionary(u => u.Id);

        // Fetch all comments and group by PostId for quick access
        var commentsGroupedByPost = (await _commentRepository.GetManyAsync())
            .GroupBy(c => c.PostId)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Order posts by CreatedDate and take the most recent ones
        var recentPosts = posts.OrderByDescending(p => p.CreatedDate).Take(count).ToList();

        // Populate each post's related data from the fetched users and comments
        foreach (var post in recentPosts)
        {
            if (users.TryGetValue(post.UserId, out var user))
            {
                post.User = user;
            }

            if (commentsGroupedByPost.TryGetValue(post.Id, out var comments))
            {
                post.Comments = comments; // Populate actual comments
            }
            else
            {
                post.Comments = new List<Comment>(); // Empty list if no comments
            }
        }

        return recentPosts;
    }


}
