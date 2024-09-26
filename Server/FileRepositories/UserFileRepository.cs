using Entities;
using RepostitoryContracts;
using System.Text.Json;
namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        try
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error initializing file: {ex.Message}");
            throw; 
        }
    }

    public async Task<User> AddAsync(User user)
    {
        try
        {
            var users = await LoadAsync();
            int maxId = users.Count > 0 ? users.Max(u => u.Id) : 0;
            user.Id = maxId + 1;
            users.Add(user);
            await SaveAsync(users);
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding user: {ex.Message}");
            throw; 
        }
    }

    public async Task UpdateAsync(User user)
    {
        try
        {
            var users = await LoadAsync();
            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Password = user.Password;
                await SaveAsync(users);
            }
            else
            {
                Console.WriteLine($"User with ID {user.Id} not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var users = await LoadAsync();
            var userToDelete = users.FirstOrDefault(u => u.Id == id);
            if (userToDelete != null)
            {
                users.Remove(userToDelete);
                await SaveAsync(users);
            }
            else
            {
                Console.WriteLine($"User with ID {id} not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting user: {ex.Message}");
            throw;
        }
    }

    public async Task<User?> GetSingleAsync(int id)
    {
        try
        {
            var users = await LoadAsync();
            return users.FirstOrDefault(u => u.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving user: {ex.Message}");
            throw;
        }
    }

    public IQueryable<User> GetMany()
    {
        try
        {
            string usersAsJson = File.ReadAllTextAsync(filePath).Result;
            List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson) ?? new List<User>();
            return users.AsQueryable();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving users: {ex.Message}");
            throw;
        }
    }

    private async Task<List<User>> LoadAsync()
    {
        try
        {
            string usersAsJson = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<User>>(usersAsJson) ?? new List<User>();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found. Creating a new one.");
            return new List<User>(); 
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Unauthorized access: {ex.Message}");
            throw;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Failed to deserialize JSON: {ex.Message}");
            throw;
        }
    }

    private async Task SaveAsync(List<User> users)
    {
        try
        {
            string usersAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(filePath, usersAsJson);
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Unauthorized access: {ex.Message}");
            throw;
        }
        catch (IOException ex)
        {
            Console.WriteLine($"IO error during save: {ex.Message}");
            throw;
        }
    }
}



