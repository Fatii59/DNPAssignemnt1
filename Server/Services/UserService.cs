using System.Collections;
using System.Security.Cryptography;
using System.Text;
using Entities;
using RepostitoryContracts;

namespace Services;


public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        // additional business logic??
        return await _userRepository.GetSingleAsync(id);
    }

    private async Task ValidateUserCreation(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName))
            throw new ArgumentException("Username cannot be empty.");
    
        if (string.IsNullOrEmpty(password) || password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters.");

        var existingUser = (await GetAllUsersAsync()).FirstOrDefault(u => u.UserName == userName);
        if (existingUser != null)
            throw new ArgumentException("Username already exists.");
    }

    public async Task<User> CreateUserAsync(string userName, string password)
    {
        await ValidateUserCreation(userName, password);
        var hashedPassword = HashPassword(password);
        Console.WriteLine($"[CreateUserAsync] Hashed Password: {hashedPassword}"); // Debug log
    
        var user = new User { UserName = userName, Password = hashedPassword };
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
    
    
  
    private string HashPassword(string password)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }



    public async Task<List<User>> GetAllUsersAsync()
    {
        var users = _userRepository.GetMany().ToList(); 
        return await Task.FromResult(users); // Return as an asynchronous operation
    }

}
