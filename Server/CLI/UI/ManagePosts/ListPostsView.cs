using Entities;
using RepostitoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository _postRepository;

    public ListPostsView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine("=== List of Posts ===");

        
        var posts = _postRepository.GetMany().ToList();

        
        if (!posts.Any())
        {
            Console.WriteLine("No posts available.");
            return;
        }

        
        foreach (var post in posts)
        {
            DisplayPost(post);
        }
    }

    private void DisplayPost(Post post)
    {
        Console.WriteLine($"Post ID: {post.Id}");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
        Console.WriteLine($"User ID: {post.UserId}");
        Console.WriteLine($"Number of Comments: {post.Comments.Count}");
        Console.WriteLine("-------------------------------");
    }
}
