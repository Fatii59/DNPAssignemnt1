
using Services;

namespace CLI.UI.ManagePosts;

    public class SinglePostView
    {
        private readonly IPostService _postService;

        public SinglePostView(IPostService postService)
        {
            _postService=postService;
        }

        
        public async Task DisplayAsync()
        {
            Console.WriteLine("=== View Post ===");

   
            Console.Write("Enter post ID: ");
            if (!int.TryParse(Console.ReadLine(), out var postId))
            {
                Console.WriteLine("Invalid post ID.");
                return;
            }

     
            try
            {
                var post = await _postService.GetPostByIdAsync(postId);
               
                Console.WriteLine($"Post ID: {post.Id}");
                Console.WriteLine($"Title: {post.Title}");
                Console.WriteLine($"Content: {post.Body}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

