using Entities;
using RepostitoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository _userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine(" Create a new user: ");
        Console.Write("Enter username: ");
        string userName = Console.ReadLine();

        if (string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("Username cannot be empty! Please try again. ");
            return;
        }
        var existingUsers = _userRepository.GetMany().ToList();
        if (existingUsers.Any(u => u.UserName == userName))
        {
            Console.WriteLine("Username already exists! Please try again. ");
            return;
        }
        
        var user= new User{UserName = userName};
        var createdUser =await _userRepository.AddAsync(user);
        
        Console.WriteLine($"User '{createdUser.UserName}' has been created!");
    }
}
