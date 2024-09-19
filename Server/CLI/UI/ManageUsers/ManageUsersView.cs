using System.Threading.Tasks;
using RepostitoryContracts;
using Entities;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    
    private readonly IUserRepository _userRepository;


    public ManageUsersView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task DisplayMenuAsync()
    {
        Console.WriteLine("== Manage Users ==");
        Console.WriteLine("1. Create User");
        Console.WriteLine("2. List Users");
        Console.WriteLine("3. Edit User");
        Console.WriteLine("4. Delete User");
        Console.Write("Enter your choice: ");
        var choice=Console.ReadLine();
        switch (choice)
        {
            case "1":
                await CreateUserAsync();
                break;
            case "2":
                await ShowListUsersViewAsync();
                break;
            case "3":
                await ShowEditUserViewAsync();
                break;
            case "4":
                await ShowDeleteUserViewAsync();
                break;
            default:
                Console.WriteLine("Invalid choice");
                break;
        }
    }
    
    private async Task ShowEditUserViewAsync()
    {
        var editUserView = new EditUserView(_userRepository);
        await editUserView.DisplayAsync();
    }

    private async Task ShowDeleteUserViewAsync()
    {
        var deleteUserView = new DeleteUserView(_userRepository);
        await deleteUserView.DisplayAsync();
    }

    private async Task ShowListUsersViewAsync()
    {
        var listUserView = new ListUsersView(_userRepository);
        await listUserView.DisplayAsync();
    }
    private async Task ListUsersAsync()
    {
        var users = _userRepository.GetMany().ToList();

        if (users.Count == 0)
        {
            Console.WriteLine("No data found");
            return;
        }
        
        Console.WriteLine("Users:");
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id}, Username: {user.UserName}");
        }
    }

    private async Task CreateUserAsync()
    {
        Console.WriteLine("== Create new User ==");
        Console.WriteLine("Enter username: ");
        string userName = Console.ReadLine();

        if (string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("Invalid username");
            return;
        }
        
        var user= new User{UserName = userName};
        await _userRepository.AddAsync(user);
        Console.WriteLine($"User '{userName}' has been created successfully");
    }
}