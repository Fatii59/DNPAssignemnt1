using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
// Ensure this namespace is correct
using Services; // Make sure this is the namespace for your service interfaces

namespace CLI.UI.ManageUsers
{
    public class ListUsersView
    {
        private readonly IUserService _userService; // Change to use IUserService

        public ListUsersView(IUserService userService) // Inject IUserService
        {
            _userService = userService;
        }

        public async Task DisplayAsync()
        {
            Console.WriteLine("Listing all users");

            // Use the UserService to get the list of users
            var users = await _userService.GetAllUsersAsync(); // Ensure this method exists in your IUserService
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
}