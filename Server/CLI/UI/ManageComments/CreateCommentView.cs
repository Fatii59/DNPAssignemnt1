
using Services;

namespace CLI.UI.ManageComments
{
    public class CreateCommentView
    {
        private readonly ICommentService _commentService;

        public CreateCommentView(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task DisplayAsync()
        {
            Console.WriteLine("Enter comment body: ");
            var body = Console.ReadLine();

            Console.WriteLine("Enter post ID: ");
            if (!int.TryParse(Console.ReadLine(), out var postId))
            {
                Console.WriteLine("Invalid post ID. Please enter a valid integer.");
                return;
            }

            Console.WriteLine("Enter user ID: ");
            if (!int.TryParse(Console.ReadLine(), out var userId))
            {
                Console.WriteLine("Invalid user ID. Please enter a valid integer.");
                return;
            }

            try
            {
                // Call CreateCommentAsync on the comment service
                var newComment = await _commentService.CreateCommentAsync(body, postId, userId);
                Console.WriteLine("Comment created successfully with ID: " + newComment.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating comment: {ex.Message}");
            }
        }
    }
}