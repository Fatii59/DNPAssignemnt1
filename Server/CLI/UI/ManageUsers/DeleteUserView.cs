using RepostitoryContracts;
using Services;

namespace CLI.UI.ManageUsers;

public class DeleteUserView
{
    private readonly IUserService _userService;

    // Injecting the UserService instead of the repository
    public DeleteUserView(IUserService userService)
    {
        _userService = userService;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine("Enter the User ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var userId))
        {
            Console.WriteLine("Invalid User ID.");
            return;
        }

        try
        {
            // Fetch user via the service
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                Console.WriteLine($"User with ID {userId} not found.");
                return;
            }

            // Confirm deletion with the user
            Console.WriteLine($"Are you sure you want to delete the user \"{user.UserName}\"? (y/n)");
            var confirmation = Console.ReadLine();
            if (confirmation?.ToLower() == "y")
            {
                // Use the service to delete the user
                await _userService.DeleteUserAsync(userId);
                Console.WriteLine("User deleted successfully.");
            }
            else
            {
                Console.WriteLine("User deletion cancelled.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
