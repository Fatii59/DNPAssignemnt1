using Entities;
 // Ensure this namespace is correct
using Services; // Include this if you have a UserService

namespace CLI.UI.ManageUsers
{
    public class EditUserView
    {
        private readonly IUserService _userService; // Change to UserService

        public EditUserView(IUserService userService) // Inject IUserService
        {
            _userService = userService;
        }

        public async Task DisplayAsync()
        {
            Console.WriteLine("Enter the User ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out var userId))
            {
                Console.WriteLine("Invalid User ID.");
                return;
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(userId); // Use the service to get the user
                if (user == null)
                {
                    Console.WriteLine($"User with ID {userId} not found.");
                    return;
                }

                DisplayUserDetails(user);

                Console.WriteLine("Enter new username (or press Enter to keep current): ");
                var newUsername = Console.ReadLine();
                if (!string.IsNullOrEmpty(newUsername)) 
                {
                    user.UserName = newUsername; // Update username if provided
                }

                Console.WriteLine("Enter new password (or press Enter to keep current): ");
                var newPassword = Console.ReadLine();
                if (!string.IsNullOrEmpty(newPassword)) 
                {
                    user.Password = newPassword; // Update password if provided
                }

                await _userService.UpdateUserAsync(user); // Use the service to update the user
                Console.WriteLine("User updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void DisplayUserDetails(User user)
        {
            Console.WriteLine("=== Current User Details ===");
            Console.WriteLine($"ID: {user.Id}");
            Console.WriteLine($"Username: {user.UserName}");
            Console.WriteLine($"Password: {new string('*', user.Password.Length)}"); // Masked password display
        }
    }
}
