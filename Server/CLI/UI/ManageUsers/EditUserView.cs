using Entities;
using RepostitoryContracts;

namespace CLI.UI.ManageUsers;

public class EditUserView
{
    private readonly IUserRepository _userRepository;

        public EditUserView(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
                
                var user = await _userRepository.GetSingleAsync(userId);
                if (user == null)
                {
                    Console.WriteLine($"User with ID {userId} not found.");
                    return;
                }

              
                DisplayUserDetails(user);

                
                Console.WriteLine("Enter new username (or press Enter to keep current): ");
                var newUsername = Console.ReadLine();
                if (!string.IsNullOrEmpty(newUsername)) user.UserName = newUsername;

                Console.WriteLine("Enter new password (or press Enter to keep current): ");
                var newPassword = Console.ReadLine();
                if (!string.IsNullOrEmpty(newPassword)) user.Password = newPassword;

                
                await _userRepository.UpdateAsync(user);
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
            Console.WriteLine($"Password: {user.Password}");
        }
    }
