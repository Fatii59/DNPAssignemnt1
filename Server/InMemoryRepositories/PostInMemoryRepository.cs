namespace InMemoryRepositories;

using Entities;
using RepostitoryContracts;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts = new List<Post>();
    
    public PostInMemoryRepository()
    {
        // dummy data
        posts = new List<Post>
        {
            new Post
            {
                Id = 1,
                Title = "Welcome to the Blog!",
                Body= "This is the first post on this blog. Feel free to leave comments!",
                UserId = 1, 
            },
            new Post
            {
                Id = 2,
                Title = "Another Interesting Post",
                Body = "This is a second post to showcase the functionality.",
                UserId = 2, 
            
            }
        };
    }

    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any() 
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }
    
    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost == null)
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove == null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }
    
    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = posts.SingleOrDefault(p => p.Id == id);
        if (post == null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        return Task.FromResult(post);
    }

    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
    

}
