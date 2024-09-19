using RepostitoryContracts;
using Entities;
using System.Threading.Tasks;
namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository _postRepository;

    public CreatePostView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine("== Create Post ==");
        Console.WriteLine("Enter Post Title:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter Post Description:");
        string body = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Title and description are required.");
            return;
        }

        var post = new Post
        {
            Title = title,
            Body = body

        };

        await _postRepository.AddAsync(post);
        Console.WriteLine($"Post '{title}' has been created.");

    }
}