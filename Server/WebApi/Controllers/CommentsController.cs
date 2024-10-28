using DTOs;
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
    public async Task<IActionResult> CreateComment(CreateCommentDTO dto)
    {
        var createdComment = await _commentService.CreateCommentAsync(dto.Body, dto.PostId, dto.UserId);
        return CreatedAtAction(nameof(GetSingleComment), new { id = createdComment.Id }, createdComment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, UpdateCommentDTO dto)
    {
        var result = await _commentService.GetCommentByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        result.Body = dto.Body;
        await _commentService.UpdateCommentAsync(result);
        return NoContent();
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
    public async Task<ActionResult<List<CommentDTO>>> GetManyComments()
    {
        var comments = await _commentService.GetAllCommentsAsync();

        if (!comments.Any())
        {
            return NotFound("No comments found.");
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
