using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> AddUser([FromBody] CreateUserDTO request)
    {
        try
        {
            await VerifyUserNameIsAvailableAsync(request.UserName);

            var created = await _userService.CreateUserAsync(request.UserName, request.Password);

            var dto = new UserDTO
            {
                Id = created.Id,
                UserName = created.UserName
            };

            return Created($"/Users/{dto.Id}", dto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

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

    private async Task VerifyUserNameIsAvailableAsync(string userName)
    {
        var existingUser = (await _userService.GetAllUsersAsync())
            .FirstOrDefault(u => u.UserName == userName);

        if (existingUser != null)
        {
            throw new InvalidOperationException("Username already exists.");
        }
    }
}