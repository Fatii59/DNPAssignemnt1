using DTOs;

namespace BlazorApp1.Models;

public class LoginResponse
{
    public string Token { get; set; }
    public UserDTO User { get; set; }
}
