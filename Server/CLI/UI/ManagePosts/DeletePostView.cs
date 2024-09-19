using RepostitoryContracts;

namespace CLI.UI.ManagePosts;

public class DeletePostView
{
    private readonly IPostRepository _postRepository;

    public DeletePostView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine("Enter the Post ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var postId))
        {
            Console.WriteLine("Invalid Post ID.");
            return;
        }

        try
        {
            
            var post = await _postRepository.GetSingleAsync(postId);
            if (post == null)
            {
                Console.WriteLine($"Post with ID {postId} not found.");
                return;
            }

            
            Console.WriteLine($"Are you sure you want to delete the post titled \"{post.Title}\"? (y/n)");
            var confirmation = Console.ReadLine();
            if (confirmation?.ToLower() == "y")
            {
                await _postRepository.DeleteAsync(postId);
                Console.WriteLine("Post deleted successfully.");
            }
            else
            {
                Console.WriteLine("Post deletion cancelled.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}