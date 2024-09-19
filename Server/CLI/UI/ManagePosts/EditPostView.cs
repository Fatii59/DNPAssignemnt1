using Entities;
using RepostitoryContracts;

namespace CLI.UI.ManagePosts;

public class EditPostView
{
  
        private readonly IPostRepository _postRepository;

        public EditPostView(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task DisplayAsync()
        {
            Console.WriteLine("Enter the Post ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out var postId))
            {
                Console.WriteLine("Invalid Post ID.");
                return;
            }

            try
            {
                
                var post = await _postRepository.GetSingleAsync(postId);
                if (post == null)
                {
                    Console.WriteLine($"Post with ID {postId} not found.");
                    return;
                }

                
                DisplayPostDetails(post);

                
                Console.WriteLine("Enter new title (or press Enter to keep current): ");
                var newTitle = Console.ReadLine();
                if (!string.IsNullOrEmpty(newTitle)) post.Title = newTitle;

                Console.WriteLine("Enter new body (or press Enter to keep current): ");
                var newBody = Console.ReadLine();
                if (!string.IsNullOrEmpty(newBody)) post.Body = newBody;

               
                await _postRepository.UpdateAsync(post);
                Console.WriteLine("Post updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void DisplayPostDetails(Post post)
        {
            Console.WriteLine("=== Current Post Details ===");
            Console.WriteLine($"ID: {post.Id}");
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"Body: {post.Body}");
            Console.WriteLine($"User ID: {post.UserId}");
            Console.WriteLine("-----------------------------");
        }
}