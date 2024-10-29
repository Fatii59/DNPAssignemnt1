using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // POST: Create a new user
    [HttpPost]
    public async Task<ActionResult<UserDTO>> AddUser([FromBody] CreateUserDTO request)
    {
        try
        {
            var created = await _userService.CreateUserAsync(request.UserName, request.Password);

            var dto = new UserDTO
            {
                Id = created.Id,
                UserName = created.UserName
            };

            return CreatedAtAction(nameof(GetUserById), new { id = dto.Id }, dto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET: Get all users or filter by username
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetMany([FromQuery] string? userName = null)
    {
        var users = await _userService.GetAllUsersAsync();

        if (!string.IsNullOrEmpty(userName))
        {
            users = users.Where(u => u.UserName.Contains(userName)).ToList();
        }

        var userDtos = users.Select(user => new UserDTO
        {
            Id = user.Id,
            UserName = user.UserName
        }).ToList();

        return Ok(userDtos);
    }

    // Optional: Get a single user by ID to verify user existence
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        var userDto = new UserDTO
        {
            Id = user.Id,
            UserName = user.UserName
        };

        return Ok(userDto);
    }
}