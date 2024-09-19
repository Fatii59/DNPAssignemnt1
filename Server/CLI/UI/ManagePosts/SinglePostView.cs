

using System;
using System.Threading.Tasks;
using RepostitoryContracts;

namespace CLI.UI.ManagePosts;

    public class SinglePostView
    {
        private readonly IPostRepository _postRepository;

        public SinglePostView(IPostRepository postRepository)
        {
            _postRepository = postRepository;
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
                var post = await _postRepository.GetSingleAsync(postId);
               
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

