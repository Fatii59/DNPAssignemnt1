
using Services;

namespace CLI.UI.ManagePosts
{
    public class CreatePostView
    {
        private readonly IPostService _postService;

        public CreatePostView(IPostService postService)
        {
            _postService = postService;
        }

        public async Task DisplayAsync()
        {
            Console.WriteLine("== Create Post ==");

            Console.WriteLine("Enter Post Title:");
            string title = Console.ReadLine();

            Console.WriteLine("Enter Post Description:");
            string body = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
            {
                Console.WriteLine("Title and description are required.");
                return;
            }

            Console.WriteLine("Enter User ID:");
            if (!int.TryParse(Console.ReadLine(), out var userId))
            {
                Console.WriteLine("Invalid user ID. Please enter a valid integer.");
                return;
            }

            try
            {
                // Call CreatePostAsync on the post service
                var post = await _postService.CreatePostAsync(title, body, userId);
                Console.WriteLine($"Post '{post.Title}' has been created with ID: {post.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating post: {ex.Message}");
            }
        }
    }
}