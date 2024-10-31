using DTOs;
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
