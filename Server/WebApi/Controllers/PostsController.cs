using DTOs;

using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IUserService _userService;

    public PostController(IPostService postService, IUserService userService)
    {
        _postService = postService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<PostDTO>> CreatePost([FromBody] CreatePostDTO request)
    {
        var user = await _userService.GetUserByIdAsync(request.UserId);
        if (user == null)
        {
            return BadRequest("Invalid UserId provided.");
        }

        var createdPost = await _postService.CreatePostAsync(request.Title, request.Body, request.UserId);

        var postDto = new PostDTO
        {
            Id = createdPost.Id,
            Title = createdPost.Title,
            Body = createdPost.Body,
            UserName = user.UserName
        };

        return Created($"/Posts/{postDto.Id}", postDto);
    }

    [HttpGet]
    public async Task<ActionResult<List<PostDTO>>> GetMany()
    {
        var posts = await _postService.GetAllPostsAsync();

        var postDtos = posts.Select(p => new PostDTO
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            UserId = p.UserId,
            UserName = "ExampleUser" // Optionally replace with actual user fetching if needed
        }).ToList();

        return Ok(postDtos);
    }
}