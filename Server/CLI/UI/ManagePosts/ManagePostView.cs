using System.Threading.Tasks;
using RepostitoryContracts;
using Services;

namespace CLI.UI.ManagePosts;

public class ManagePostView
{
    private readonly IPostService _postService;


    public ManagePostView(IPostService postService)
    {
        _postService = postService;
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
        var editPostView = new EditPostView(_postService);
        await editPostView.DisplayAsync();
    }
    
    private async Task ShowDeletePostViewAsync()
    {
        var deletePostView = new DeletePostView(_postService);
        await deletePostView.DisplayAsync();
    }
    
    private async Task ShowSinglePostViewAsync()
    {
        var singlePostView = new SinglePostView(_postService);
        await singlePostView.DisplayAsync();
    }
    
    private async Task ShowCreatePostViewAsync()
    {
        var createPostView = new CreatePostView(_postService);
        await createPostView.DisplayAsync();
    }

    private async Task ShowListPostsViewAsync()
    {
        var listPostView = new ListPostsView(_postService);
        await listPostView.DisplayAsync();
    }

}