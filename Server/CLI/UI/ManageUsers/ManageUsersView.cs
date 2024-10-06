using System.Threading.Tasks;
using RepostitoryContracts;
using Entities;
using Services;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    
    private readonly IUserService _userService;

    public ManageUsersView(IUserService userService) 
    {
        _userService = userService;
    }

    public async Task DisplayMenuAsync()
    {
        Console.WriteLine("== Manage Users ==");
        Console.WriteLine("1. Create User");
        Console.WriteLine("2. List Users");
        Console.WriteLine("3. Edit User");
        Console.WriteLine("4. Delete User");
        Console.Write("Enter your choice: ");
        var choice = Console.ReadLine();
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
        var editUserView = new EditUserView(_userService);  
        await editUserView.DisplayAsync();
    }

    private async Task ShowDeleteUserViewAsync()
    {
        var deleteUserView = new DeleteUserView(_userService);  
        await deleteUserView.DisplayAsync();
    }

    private async Task ShowListUsersViewAsync()
    {
        var listUserView = new ListUsersView(_userService);  
        await listUserView.DisplayAsync();
    }

    private async Task ListUsersAsync()
    {
        var users = await _userService.GetAllUsersAsync(); 

        if (users == null || users.Count == 0)
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

        string userName;
        while (true)
        {
            Console.Write("Enter username: ");
            userName = Console.ReadLine();

            if (string.IsNullOrEmpty(userName))
            {
                Console.WriteLine("Invalid username. Please try again.");
                continue;
            }

            // Check if the username already exists
            var existingUsers = await _userService.GetAllUsersAsync();
            if (existingUsers.Any(u => u.UserName == userName))
            {
                Console.WriteLine("Username already exists. Please choose another.");
            }
            else
            {
                break; // Valid username, exit the loop
            }
        }

        Console.Write("Enter password (at least 6 characters): ");
        string password = Console.ReadLine();

        // Check password length
        if (string.IsNullOrEmpty(password) || password.Length < 6)
        {
            Console.WriteLine("Invalid password. It must be at least 6 characters.");
            return;
        }

        await _userService.CreateUserAsync(userName, password);
        Console.WriteLine($"User '{userName}' has been created successfully");
    }




}