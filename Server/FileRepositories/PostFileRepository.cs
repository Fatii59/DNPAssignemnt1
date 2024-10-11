namespace FileRepositories;

using System.Text.Json;
using Entities;
using RepostitoryContracts;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        try
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]"); 
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error initializing file: {ex.Message}");
            throw; 
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        try
        {
            var posts = await LoadAsync();
            int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 0;
            post.Id = maxId + 1;
            posts.Add(post);
            await SaveAsync(posts);
            return post;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding post: {ex.Message}");
            throw; 
        }
    }

    public async Task UpdateAsync(Post post)
    {
        try
        {
            var posts = await LoadAsync();
            var existingPost = posts.FirstOrDefault(p => p.Id == post.Id);
            if (existingPost != null)
            {
                existingPost.Title = post.Title;
                existingPost.Body = post.Body;
                await SaveAsync(posts);
            }
            else
            {
                Console.WriteLine($"Post with ID {post.Id} not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating post: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var posts = await LoadAsync();
            var postToDelete = posts.FirstOrDefault(p => p.Id == id);
            if (postToDelete != null)
            {
                posts.Remove(postToDelete);
                await SaveAsync(posts);
            }
            else
            {
                Console.WriteLine($"Post with ID {id} not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting post: {ex.Message}");
            throw;
        }
    }

    public async Task<Post?> GetSingleAsync(int id)
    {
        try
        {
            var posts = await LoadAsync();
            return posts.FirstOrDefault(p => p.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving post: {ex.Message}");
            throw;
        }
    }

    public async Task<IQueryable<Post>> GetMany()
    {
        var posts = await LoadAsync(); // Load posts from file
        return posts.AsQueryable();     // Return as IQueryable
    }


    private async Task<List<Post>> LoadAsync()
    {
        try
        {
            string postsAsJson = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Post>>(postsAsJson) ?? new List<Post>();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found. Creating a new one.");
            return new List<Post>();
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Unauthorized access: {ex.Message}");
            throw;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Failed to deserialize JSON: {ex.Message}");
            throw;
        }
    }

    private async Task SaveAsync(List<Post> posts)
    {
        try
        {
            string postsAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(filePath, postsAsJson);
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Unauthorized access: {ex.Message}");
            throw;
        }
        catch (IOException ex)
        {
            Console.WriteLine($"IO error during save: {ex.Message}");
            throw;
        }
    }
}
