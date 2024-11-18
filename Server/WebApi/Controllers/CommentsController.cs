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

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

  
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO request)
    {
        // Get the user ID from the authenticated user's claims
        var userIdClaim = User.FindFirst("Id");
        if (userIdClaim == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var userId = int.Parse(userIdClaim.Value);

        // Use the userId from the claims instead of the client-supplied ID
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
    public async Task<ActionResult<List<CommentDTO>>> GetManyComments(int? postId = null)
    {
        var comments = await _commentService.GetAllCommentsAsync();
    
        if (postId.HasValue)
        {
            comments = comments.Where(c => c.PostId == postId.Value).ToList();
        }

        var commentDtos = comments.Select(comment => new CommentDTO
        {
            Id = comment.Id,
            Body = comment.Body,
            PostId = comment.PostId,
            UserId = comment.UserId
        }).ToList();

        return Ok(commentDtos);
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
