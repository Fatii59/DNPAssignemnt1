using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepostitoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment(CreateCommentDTO dto)
    {
        var comment = new Comment
        {
            Body = dto.Body,
            PostId = dto.PostId,
            UserId = dto.UserId
        };

        var createdComment = await _commentRepository.AddAsync(comment);
        return CreatedAtAction(nameof(GetSingleComment), new { id = createdComment.Id }, createdComment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, UpdateCommentDTO dto)
    {
        var comment = await _commentRepository.GetSingleAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        comment.Body = dto.Body;
        await _commentRepository.UpdateAsync(comment);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDTO>> GetSingleComment(int id)
    {
        try
        {
            // Call the repository to get the comment
            var comment = await _commentRepository.GetSingleAsync(id);

            // Check if the comment was found
            if (comment == null)
            {
                return NotFound($"Comment with ID {id} not found.");
            }

            // Map the Comment to CommentDTO
            var commentDto = new CommentDTO
            {
                Id = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                UserId = comment.UserId
            };

            return Ok(commentDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving comment: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving the comment.");
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CommentDTO>>> GetManyComments()
    {
        try
        {
            // Call the asynchronous method to retrieve comments
            var comments = await _commentRepository.GetManyAsync();

            if (!comments.Any())
            {
                return NotFound("No comments found.");
            }

            // Project comments into DTOs
            var commentDtos = comments.Select(comment => new CommentDTO
            {
                Id = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                UserId = comment.UserId
            }).ToList(); // Materialize the query here

            return Ok(commentDtos);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving comments: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving the comments.");
        }
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _commentRepository.GetSingleAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        await _commentRepository.DeleteAsync(id);
        return NoContent();
    }
}
