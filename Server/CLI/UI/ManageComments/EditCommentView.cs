using Entities;
using RepostitoryContracts;

namespace CLI.UI.ManageComments;

public class EditCommentView
{
    private readonly ICommentRepository _commentRepository;

    public EditCommentView(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine("Enter the Comment ID to edit: ");
        if (!int.TryParse(Console.ReadLine(), out var commentId))
        {
            Console.WriteLine("Invalid Comment ID.");
            return;
        }

        try
        {
            
            var comment = await _commentRepository.GetSingleAsync(commentId);
            if (comment == null)
            {
                Console.WriteLine($"Comment with ID {commentId} not found.");
                return;
            }

            
            DisplayCommentDetails(comment);

            
            Console.WriteLine("Enter new body (or press Enter to keep current): ");
            var newBody = Console.ReadLine();
            if (!string.IsNullOrEmpty(newBody)) comment.Body = newBody;

            
            await _commentRepository.UpdateAsync(comment);
            Console.WriteLine("Comment updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void DisplayCommentDetails(Comment comment)
    {
        Console.WriteLine("=== Current Comment Details ===");
        Console.WriteLine($"ID: {comment.Id}");
        Console.WriteLine($"Body: {comment.Body}");
        Console.WriteLine($"Post ID: {comment.PostId}");
        Console.WriteLine($"User ID: {comment.UserId}");
        Console.WriteLine("-----------------------------");
    }
}