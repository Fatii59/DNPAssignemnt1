using Entities;

using Services; 

namespace CLI.UI.ManageUsers
{
    public class EditUserView
    {
        private readonly IUserService _userService;

        public EditUserView(IUserService userService) 
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
                var user = await _userService.GetUserByIdAsync(userId); 
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

                await _userService.UpdateUserAsync(user); 
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
