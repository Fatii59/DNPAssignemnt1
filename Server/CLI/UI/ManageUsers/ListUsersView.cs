using System;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using RepostitoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository _userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine("Listing all users");

        var users = _userRepository.GetMany().ToList();
        if (users.Count == 0)
        {
            Console.WriteLine("There are no users");
            return;
        }

        foreach (var user in users)
        {
           Console.WriteLine($"ID: {user.Id}, Username: {user.UserName}"); 
        }
    }
}