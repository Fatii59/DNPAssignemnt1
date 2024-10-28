
using Entities;
using Services;

namespace CLI.UI.ManageComments
{
    public class ListCommentsView
    {
        private readonly ICommentService _commentService;

        public ListCommentsView(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task DisplayAsync()
        {
            Console.WriteLine("=== List of Comments ===");

            var comments = await _commentService.GetAllCommentsAsync();
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
}