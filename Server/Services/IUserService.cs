using Entities;

namespace Services;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(string userName, string password);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
    Task<List<User>> GetAllUsersAsync();
}