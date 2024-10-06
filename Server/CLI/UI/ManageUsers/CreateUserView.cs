using Entities;
using RepostitoryContracts;
using Services;


namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserService _userService;

    public CreateUserView(IUserService userService)
    {
        _userService = userService;
    }

    public async Task DisplayAsync()
    {
        Console.Write("Enter username: ");
        string userName = Console.ReadLine();
        
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        try
        {
            var createdUser = await _userService.CreateUserAsync(userName, password);
            Console.WriteLine($"User '{createdUser.UserName}' has been created!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
