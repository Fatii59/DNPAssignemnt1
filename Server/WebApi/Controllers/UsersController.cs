using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepostitoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // POST: /Users
    [HttpPost]
    public async Task<ActionResult<UserDTO>> AddUser([FromBody] CreateUserDTO request) // Fixed DTO naming
    {
        // Await the async method for user name availability check
        await VerifyUserNameIsAvailableAsync(request.UserName);

        User user = new(request.UserName, request.Password);
        User created = await _userRepository.AddAsync(user);

        UserDTO dto = new UserDTO
        {
            Id = created.Id,
            UserName = created.UserName
        };

        return Created($"/Users/{dto.Id}", dto);
    }

    // GET: /Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetMany([FromQuery] string? userName = null)
    {
        var users = _userRepository.GetMany();

        // Apply the filter if a username query is provided
        if (!string.IsNullOrEmpty(userName))
        {
            users = users.Where(u => u.UserName.Contains(userName));
        }

        // Project to UserDTOs asynchronously
        var userDtos = await Task.Run(() => users.Select(user => new UserDTO
        {
            Id = user.Id,
            UserName = user.UserName
        }).ToList());

        return Ok(userDtos);
    }

    // Verify asynchronously if the username is available
    private async Task VerifyUserNameIsAvailableAsync(string userName)
    {
        var existingUser = await Task.Run(() => _userRepository.GetMany()
                                   .FirstOrDefault(u => u.UserName == userName));

        if (existingUser != null)
        {
            // You can throw a custom exception or return an HTTP response with a 400 status
            throw new InvalidOperationException("Username already exists.");
        }
    }
}

