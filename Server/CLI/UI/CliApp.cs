using RepostitoryContracts;
using InMemoryRepositories;
using System.Threading.Tasks;
using CLI.UI.ManageUsers;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository _userRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        _userRepository = userRepository;
        _commentRepository = commentRepository;
        _postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        Console.WriteLine("==  User Management ==");
        Console.WriteLine("1. create a new user");
        Console.WriteLine("Enter your choice:  ");
        var choice = Console.ReadLine();
        if (choice == "1")
        {
            var createUserView = new CreateUserView(_userRepository);
            await createUserView.DisplayAsync();
        }
        
        var manageUsersView = new ManageUsersView(_userRepository);
        await manageUsersView.DisplayMenuAsync();
    }
}