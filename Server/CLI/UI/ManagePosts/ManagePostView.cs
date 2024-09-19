using System.Threading.Tasks;
using RepostitoryContracts;
namespace CLI.UI.ManagePosts;

public class ManagePostView
{
    private readonly IPostRepository _postRepository;


    public ManagePostView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    
    public async Task DisplayMenuAsync()
    {
        Console.WriteLine("=== Manage Posts ===");
        Console.WriteLine("1. Create Post");
        Console.WriteLine("2. List Posts");
        Console.WriteLine("3. View Post");
        Console.WriteLine("4. Edit Post");
        Console.WriteLine("5. Delete Post");
        Console.Write("Enter your choice: ");
            
        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                await ShowCreatePostViewAsync();
                break;
            case "2":
                await ShowListPostsViewAsync();
                break;
            case "3":
                await ShowSinglePostViewAsync();
                break;
            case "4":
                await ShowEditPostViewAsync();
                break;
            case "5":
                await ShowDeletePostViewAsync();
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }
    
    private async Task ShowEditPostViewAsync()
    {
        var editPostView = new EditPostView(_postRepository);
        await editPostView.DisplayAsync();
    }
    
    private async Task ShowDeletePostViewAsync()
    {
        var deletePostView = new DeletePostView(_postRepository);
        await deletePostView.DisplayAsync();
    }
    
    private async Task ShowSinglePostViewAsync()
    {
        var singlePostView = new SinglePostView(_postRepository);
        await singlePostView.DisplayAsync();
    }
    
    private async Task ShowCreatePostViewAsync()
    {
        var createPostView = new CreatePostView(_postRepository);
        await createPostView.DisplayAsync();
    }

    private async Task ShowListPostsViewAsync()
    {
        var listPostView = new ListPostsView(_postRepository);
        await listPostView.DisplayAsync();
    }

}