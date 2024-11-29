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
        var userIdClaim = User.FindFirst("Id");
        if (userIdClaim == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        if (!int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized("Invalid user ID.");
        }

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
    public async Task<ActionResult<List<PostDTO>>> GetMany(int page = 1, int pageSize = 10)
    {
        var posts = await _postService.GetAllPostsAsync();

        var pagedPosts = posts
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(post => new PostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                UserName = post.User?.UserName ?? "Unknown",
                CommentCount = post.Comments.Count
            })
            .ToList();

        return Ok(pagedPosts);
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

        var postDto = new PostDTO
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId,
            UserName = post.User?.UserName ?? "Unknown",
            CommentCount = post.Comments.Count
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
