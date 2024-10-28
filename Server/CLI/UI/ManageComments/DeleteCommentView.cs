using RepostitoryContracts;
using Services;

namespace CLI.UI.ManageComments;

public class DeleteCommentView
{
    private readonly  ICommentService _commentService;

    public DeleteCommentView( ICommentService commentService)
    {
        _commentService=commentService;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine("Enter the comment ID to delete: ");
        var id = int.Parse(Console.ReadLine());

        try
        {
            await _commentService.DeleteCommentAsync(id);
            Console.WriteLine("Comment deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}