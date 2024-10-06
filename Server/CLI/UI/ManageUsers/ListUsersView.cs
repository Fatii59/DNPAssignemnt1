using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

using Services; 

namespace CLI.UI.ManageUsers
{
    public class ListUsersView
    {
        private readonly IUserService _userService;

        public ListUsersView(IUserService userService) 
        {
            _userService = userService;
        }

        public async Task DisplayAsync()
        {
            Console.WriteLine("Listing all users");

            // Use the UserService to get the list of users
            var users = await _userService.GetAllUsersAsync(); 
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