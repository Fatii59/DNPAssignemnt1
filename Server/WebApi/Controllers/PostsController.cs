using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepostitoryContracts;

namespace WebApi.Controllers;



[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public PostController(IPostRepository postRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<PostDTO>> CreatePost([FromBody] CreatePostDTO request)
    {
        // Create a new post entity with the incoming data
        var post = new Post
        {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId
        };

        // Add the post to the database and save changes
        var createdPost = await _postRepository.AddAsync(post);

        // Fetch the user from the repository
        var user = await _userRepository.GetSingleAsync(post.UserId); // Make sure this method exists

        // If the user was not found, return a bad request
        if (user == null)
        {
            return BadRequest("Invalid UserId provided.");
        }

        // Prepare the PostDTO object to return
        var postDto = new PostDTO
        {
            Id = createdPost.Id,
            Title = createdPost.Title,
            Body = createdPost.Body,
            UserName = user.UserName  // Now this should work as the user is properly loaded
        };

        return Created($"/Posts/{postDto.Id}", postDto);
    }

    [HttpGet]
    public async Task<ActionResult<List<PostDTO>>> GetMany()
    {
        try
        {
            var posts = await _postRepository.GetMany();

            // Transform posts into DTOs (if necessary)
            var postDtos = posts.Select(p => new PostDTO
            {
                Id = p.Id,
                Title = p.Title,
                Body = p.Body,
                UserId = p.UserId,
                UserName = "ExampleUser" // This should be replaced with actual user fetching if needed
            }).ToList();

            return Ok(postDtos); // Return the DTOs
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving posts: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving posts.");
        }
    }


}



