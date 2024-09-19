using Entities;
using RepostitoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private readonly ICommentRepository _commentRepository;

    public CreateCommentView(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine("Enter comment body: ");
        var body = Console.ReadLine();

        Console.WriteLine("Enter post ID: ");
        var postId = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter user ID: ");
        var userId = int.Parse(Console.ReadLine());

        var newComment = new Comment(body, postId, userId);
        await _commentRepository.AddAsync(newComment);

        Console.WriteLine("Comment created successfully.");
    }
}