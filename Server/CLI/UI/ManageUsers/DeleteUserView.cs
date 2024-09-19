using RepostitoryContracts;

namespace CLI.UI.ManageUsers;

public class DeleteUserView
{
    private readonly IUserRepository _userRepository;

    public DeleteUserView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
         
            var user = await _userRepository.GetSingleAsync(userId);
            if (user == null)
            {
                Console.WriteLine($"User with ID {userId} not found.");
                return;
            }

            
            Console.WriteLine($"Are you sure you want to delete the user \"{user.UserName}\"? (y/n)");
            var confirmation = Console.ReadLine();
            if (confirmation?.ToLower() == "y")
            {
                await _userRepository.DeleteAsync(userId);
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