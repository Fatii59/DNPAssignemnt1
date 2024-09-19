using Entities;
using RepostitoryContracts;

namespace CLI.UI.ManageComments;

public class SingleCommentView
{
    private readonly ICommentRepository _commentRepository;

    public SingleCommentView(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine("Enter comment ID: ");
        var id = int.Parse(Console.ReadLine());

        try
        {
            var comment = await _commentRepository.GetSingleAsync(id);
            DisplayComment(comment);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void DisplayComment(Comment comment)
    {
        Console.WriteLine($"Comment ID: {comment.Id}");
        Console.WriteLine($"Body: {comment.Body}");
        Console.WriteLine($"Post ID: {comment.PostId}");
        Console.WriteLine($"User ID: {comment.UserId}");
    }
}