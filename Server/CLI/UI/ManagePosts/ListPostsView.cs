using Entities;
using Services;

namespace CLI.UI.ManagePosts
{
    public class ListPostsView
    {
        private readonly IPostService _postService;

        public ListPostsView(IPostService postService)
        {
            _postService = postService;
        }

        public async Task DisplayAsync()
        {
            Console.WriteLine("=== List of Posts ===");

            var posts = await _postService.GetAllPostsAsync();
            if (!posts.Any())
            {
                Console.WriteLine("No posts available.");
                return;
            }

            foreach (var post in posts)
            {
                DisplayPost(post);
            }
        }

        private void DisplayPost(Post post)
        {
            Console.WriteLine($"Post ID: {post.Id}");
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"Body: {post.Body}");
            Console.WriteLine($"User ID: {post.UserId}");
            Console.WriteLine($"Number of Comments: {post.Comments.Count}");
            Console.WriteLine("-------------------------------");
        }
    }
}