namespace BlazorApp1.Services;

using DTOs;
public interface IUserService
{
    Task<UserDTO> AddUserAsync(CreateUserDTO request);
    Task<UserDTO> GetUserByIdAsync(int id);
    Task<IEnumerable<UserDTO>> GetUsersAsync(string? userName = null);
}
