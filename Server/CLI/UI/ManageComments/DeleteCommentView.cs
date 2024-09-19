using RepostitoryContracts;

namespace CLI.UI.ManageComments;

public class DeleteCommentView
{
    private readonly ICommentRepository _commentRepository;

    public DeleteCommentView(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine("Enter the comment ID to delete: ");
        var id = int.Parse(Console.ReadLine());

        try
        {
            await _commentRepository.DeleteAsync(id);
            Console.WriteLine("Comment deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}