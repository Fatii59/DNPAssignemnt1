using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IPostService _postService;

    public CommentsController(ICommentService commentService, IPostService postService)
    {
        _commentService = commentService;
        _postService = postService;
    }

  
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO request)
    {
        if (string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest("Comment body cannot be empty.");
        }

        var postExists = await _postService.GetPostByIdAsync(request.PostId) != null;
        if (!postExists)
        {
            return NotFound($"Post with ID {request.PostId} does not exist.");
        }

        var userIdClaim = User.FindFirst("Id");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized("User is not authenticated.");
        }

        var createdComment = await _commentService.CreateCommentAsync(request.Body, request.PostId, userId);

        return CreatedAtAction(nameof(GetSingleComment), new { id = createdComment.Id }, createdComment);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDTO>> GetSingleComment(int id)
    {
        var comment = await _commentService.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return NotFound($"Comment with ID {id} not found.");
        }

        var commentDto = new CommentDTO
        {
            Id = comment.Id,
            Body = comment.Body,
            PostId = comment.PostId,
            UserId = comment.UserId
        };

        return Ok(commentDto);
    }

    [HttpGet]
    [HttpGet]
    public async Task<ActionResult<List<CommentDTO>>> GetManyComments(int? postId = null, int page = 1, int pageSize = 10)
    {
        var comments = await _commentService.GetAllCommentsAsync();

        if (postId.HasValue)
        {
            comments = comments.Where(c => c.PostId == postId.Value).ToList();
        }

        var pagedComments = comments
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(comment => new CommentDTO
            {
                Id = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                UserId = comment.UserId
            }).ToList();

        return Ok(pagedComments);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _commentService.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        await _commentService.DeleteCommentAsync(id);
        return NoContent();
    }
}
