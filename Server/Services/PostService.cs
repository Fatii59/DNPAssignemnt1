namespace Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using RepostitoryContracts;


    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _postRepository.GetSingleAsync(id);
        }

        public async Task<Post> CreatePostAsync(string title, string body, int userId)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("Title cannot be empty.");
            if (string.IsNullOrEmpty(body))
                throw new ArgumentException("Body cannot be empty.");

            var post = new Post { Title = title, Body = body, UserId = userId };
            return await _postRepository.AddAsync(post);
        }

        public async Task UpdatePostAsync(Post post)
        {
            var existingPost = await _postRepository.GetSingleAsync(post.Id);
            if (existingPost == null)
                throw new ArgumentException("Post not found.");

            await _postRepository.UpdateAsync(post);
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _postRepository.GetSingleAsync(id);
            if (post == null)
                throw new ArgumentException($"Post with ID {id} does not exist.");

            await _postRepository.DeleteAsync(id);
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetMany();
            return posts.ToList();
        }
    }
