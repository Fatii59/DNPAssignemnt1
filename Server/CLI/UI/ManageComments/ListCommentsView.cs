using Entities;
using RepostitoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView
{
    private readonly ICommentRepository _commentRepository;

    public ListCommentsView(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine("=== List of Comments ===");

        var comments = _commentRepository.GetMany().ToList();

        if (!comments.Any())
        {
            Console.WriteLine("No comments available.");
            return;
        }

        foreach (var comment in comments)
        {
            DisplayComment(comment);
        }
    }

    private void DisplayComment(Comment comment)
    {
        Console.WriteLine($"Comment ID: {comment.Id}");
        Console.WriteLine($"Body: {comment.Body}");
        Console.WriteLine($"Post ID: {comment.PostId}");
        Console.WriteLine($"User ID: {comment.UserId}");
        Console.WriteLine("----------------------------");
    }
}