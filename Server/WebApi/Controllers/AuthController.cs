using DTOs;
using Microsoft.AspNetCore.Mvc;
using RepostitoryContracts;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await Task.Run(() => 
                _userRepository.GetMany().FirstOrDefault(u => u.UserName == loginRequest.UserName)
            );

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var hashedInputPassword = HashPassword(loginRequest.Password);
            Console.WriteLine($"[Login] Hashed Input Password: {hashedInputPassword}"); // Debug log
            Console.WriteLine($"[Login] Stored Password Hash: {user.Password}"); // Debug log

            if (user.Password != hashedInputPassword)
            {
                return Unauthorized("Invalid password.");
            }

            var userDto = new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                PostIds = user.Posts.Select(p => p.Id).ToList(),
                CommentIds = user.Comments.Select(c => c.Id).ToList()
            };

            return Ok(userDto);
        }


        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}