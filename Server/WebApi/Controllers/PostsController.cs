using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IUserService _userService;

    public PostController(IPostService postService, IUserService userService)
    {
        _postService = postService;
        _userService = userService;
    }

    // Create a new post
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<PostDTO>> CreatePost([FromBody] CreatePostDTO request)
    {
        Console.WriteLine($"Authorization Header: {Request.Headers["Authorization"]}");

        var userIdClaim = User.FindFirst("Id");
        if (userIdClaim == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var userId = int.Parse(userIdClaim.Value);
        Console.WriteLine($"Creating post for User ID: {userId}");

        var createdPost = await _postService.CreatePostAsync(request.Title, request.Body, userId);

        var postDto = new PostDTO
        {
            Id = createdPost.Id,
            Title = createdPost.Title,
            Body = createdPost.Body,
            UserId = userId,
            UserName = User.Identity?.Name ?? "Unknown"
        };

        return CreatedAtAction(nameof(GetPostById), new { id = postDto.Id }, postDto);
    }



    // Get all posts
    [HttpGet]
    public async Task<ActionResult<List<PostDTO>>> GetMany()
    {
        var posts = await _postService.GetAllPostsAsync();
    
        // Dictionary to cache user names by user ID
        var userCache = new Dictionary<int, string>();
    
        var postDtos = new List<PostDTO>();
        foreach (var post in posts)
        {
            // Check if the user's name is already in the cache
            if (!userCache.TryGetValue(post.UserId, out var userName))
            {
                // If not in cache, fetch from the service and store in the cache
                var user = await _userService.GetUserByIdAsync(post.UserId);
                userName = user?.UserName ?? "Unknown";
                userCache[post.UserId] = userName;
            }

            postDtos.Add(new PostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                UserName = userName
            });
        }

        return Ok(postDtos);
    }

    // Get a single post by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDTO>> GetPostById(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound($"Post with ID {id} not found.");
        }

        var user = await _userService.GetUserByIdAsync(post.UserId);
        var postDto = new PostDTO
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId,
            UserName = user?.UserName ?? "Unknown"
        };

        return Ok(postDto);
    }
    
    [HttpGet("recent")]
    public async Task<ActionResult<List<PostDTO>>> GetRecentPosts(int count = 5)
    {
        var recentPosts = await _postService.GetRecentPostsAsync(count);

        // Map `Post` entities to `PostDTOs`
        var postDtos = recentPosts.Select(post => new PostDTO
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId,
            UserName = post.User?.UserName ?? "Unknown",
            CommentCount = post.Comments.Count, // Accurate comment count
            CreatedDate = post.CreatedDate
        }).ToList();

        return Ok(postDtos);
    }







    // Update an existing post
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDTO dto)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound($"Post with ID {id} not found.");
        }

        post.Title = dto.Title;
        post.Body = dto.Body;

        await _postService.UpdatePostAsync(post);
        return NoContent();
    }

    // Delete a post by ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound($"Post with ID {id} not found.");
        }

        await _postService.DeletePostAsync(id);
        return NoContent();
    }
}
