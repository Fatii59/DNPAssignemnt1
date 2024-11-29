using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Services;

using System.Threading.Tasks;
using Entities;
using RepostitoryContracts;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetSingleAsync(id);
    }

    private async Task ValidateUserCreation(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName))
            throw new ArgumentException("Username cannot be empty.");
    
        if (string.IsNullOrEmpty(password) || password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters.");

        var existingUser = await _userRepository.GetMany()
            .AnyAsync(u => u.UserName.ToLower() == userName.ToLower());

        if (existingUser)
            throw new ArgumentException("Username already exists.");
    }

    public async Task<User> CreateUserAsync(string userName, string password)
    {
        await ValidateUserCreation(userName, password);
        var hashedPassword = HashPassword(password);
        var user = new User(userName, hashedPassword);
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
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetMany().ToListAsync();
    }
}
