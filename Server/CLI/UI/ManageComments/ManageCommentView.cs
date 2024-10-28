using RepostitoryContracts;
using Services;

namespace CLI.UI.ManageComments;

public class ManageCommentView
{
 
        private readonly ICommentService _commentService;

        public ManageCommentView(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task DisplayMenuAsync()
        {
            Console.WriteLine("=== Manage Comments ===");
            Console.WriteLine("1. Create Comment");
            Console.WriteLine("2. List Comments");
            Console.WriteLine("3. View Comment");
            Console.WriteLine("4. Edit Comment");
            Console.WriteLine("5. Delete Comment");
            Console.Write("Enter your choice: ");
            
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ShowCreateCommentViewAsync();
                    break;
                case "2":
                    await ShowListCommentsViewAsync();
                    break;
                case "3":
                    await ShowSingleCommentViewAsync();
                    break;
                case "4":
                    await ShowEditCommentViewAsync();
                    break;
                case "5":
                    await ShowDeleteCommentViewAsync();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        private async Task ShowEditCommentViewAsync()
        {
            var editCommentView = new EditCommentView(_commentService);
            await editCommentView.DisplayAsync();
        }
        private async Task ShowCreateCommentViewAsync()
        {
            var createCommentView = new CreateCommentView(_commentService);
            await createCommentView.DisplayAsync();
        }

        private async Task ShowListCommentsViewAsync()
        {
            var listCommentsView = new ListCommentsView(_commentService);
            await listCommentsView.DisplayAsync();
        }

        private async Task ShowSingleCommentViewAsync()
        {
            var singleCommentView = new SingleCommentView(_commentService);
            await singleCommentView.DisplayAsync();
        }

        private async Task ShowDeleteCommentViewAsync()
        {
            var deleteCommentView = new DeleteCommentView(_commentService);
            await deleteCommentView.DisplayAsync();
        }
    }
