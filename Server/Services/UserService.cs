using System.Collections;
using Entities;
using RepostitoryContracts;

namespace Services;

// Create a UserService class
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        // You could add any additional business logic here if needed
        return await _userRepository.GetSingleAsync(id);
    }

    public async Task<User> CreateUserAsync(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName))
            throw new ArgumentException("Username cannot be empty.");

        if (string.IsNullOrEmpty(password) || password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters.");

        var existingUsers = _userRepository.GetMany().ToList();
        if (existingUsers.Any(u => u.UserName == userName))
            throw new ArgumentException("Username already exists.");

        var user = new User { UserName = userName, Password = HashPassword(password) };
        return await _userRepository.AddAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        var existingUser = await _userRepository.GetSingleAsync(user.Id);
        if (existingUser == null)
            throw new ArgumentException("User not found.");

        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetSingleAsync(id);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {id} does not exist.");
        }
        
        await _userRepository.DeleteAsync(id);
    }

    // Example utility function for password hashing
    private string HashPassword(string password)
    {
        // For now, you can return plain text, but switch to bcrypt/Hashing in Step 2.
        return password;
    }


    public async Task<List<User>> GetAllUsersAsync()
    {
        // GetMany() returns IQueryable, so we need to materialize it into a List
        var users = _userRepository.GetMany().ToList(); // Executes the query
        return await Task.FromResult(users); // Return as an asynchronous operation
    }

}
