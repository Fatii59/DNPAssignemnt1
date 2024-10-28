
using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using Services;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserService _userService;
    private readonly ICommentService _commentService;
    private readonly IPostService _postService; 

    public CliApp(IUserService userService, ICommentService commentService,  IPostService postService)
    {
        _userService = userService;
        _commentService = commentService;
        _postService = postService;
    }

    public async Task StartAsync()
    {
        bool exitApp = false;

        while (!exitApp)
        {
            Console.WriteLine("=== Welcome to the CLI Application ===");
            Console.WriteLine("1. Manage Users");
            Console.WriteLine("2. Manage Posts");
            Console.WriteLine("3. Manage Comments");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await DisplayUserManagementMenuAsync();
                    break;
                case "2":
                    await DisplayPostManagementMenuAsync();
                    break;
                case "3":
                    await DisplayCommentManagementMenuAsync();
                    break;
                case "0":
                    exitApp = true;
                    Console.WriteLine("Exiting the application...");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private async Task DisplayUserManagementMenuAsync()
    {
        var manageUsersView = new ManageUsersView(_userService);  // Pass UserService instead of IUserRepository
        await manageUsersView.DisplayMenuAsync();
    }

    private async Task DisplayPostManagementMenuAsync()
    {
        var managePostsView = new ManagePostView(_postService);
        await managePostsView.DisplayMenuAsync();
    }

    private async Task DisplayCommentManagementMenuAsync()
    {
        var manageCommentsView = new ManageCommentView(_commentService);
        await manageCommentsView.DisplayMenuAsync();
    }
}
