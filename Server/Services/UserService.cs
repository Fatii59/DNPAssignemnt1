using System.Collections;
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

  
    private string HashPassword(string password)
    {
     
        return password;
    }


    public async Task<List<User>> GetAllUsersAsync()
    {
        var users = _userRepository.GetMany().ToList(); 
        return await Task.FromResult(users); // Return as an asynchronous operation
    }

}
