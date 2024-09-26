namespace FileRepositories;

using System.Text.Json;
using Entities;
using RepostitoryContracts;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
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

    public async Task<Comment> AddAsync(Comment comment)
    {
        try
        {
            var comments = await LoadAsync();
            int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0;
            comment.Id = maxId + 1;
            comments.Add(comment);
            await SaveAsync(comments);
            return comment;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding comment: {ex.Message}");
            throw; 
        }
    }

    public async Task UpdateAsync(Comment comment)
    {
        try
        {
            var comments = await LoadAsync();
            var existingComment = comments.FirstOrDefault(c => c.Id == comment.Id);
            if (existingComment != null)
            {
                existingComment.Body = comment.Body;
                await SaveAsync(comments);
            }
            else
            {
                Console.WriteLine($"Comment with ID {comment.Id} not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating comment: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var comments = await LoadAsync();
            var commentToDelete = comments.FirstOrDefault(c => c.Id == id);
            if (commentToDelete != null)
            {
                comments.Remove(commentToDelete);
                await SaveAsync(comments);
            }
            else
            {
                Console.WriteLine($"Comment with ID {id} not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting comment: {ex.Message}");
            throw;
        }
    }

    public async Task<Comment?> GetSingleAsync(int id)
    {
        try
        {
            var comments = await LoadAsync();
            return comments.FirstOrDefault(c => c.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving comment: {ex.Message}");
            throw;
        }
    }

    public IQueryable<Comment> GetMany()
    {
        try
        {
            string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
            List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson) ?? new List<Comment>();
            return comments.AsQueryable();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving comments: {ex.Message}");
            throw;
        }
    }

  

    private async Task<List<Comment>> LoadAsync()
    {
        try
        {
            string commentsAsJson = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Comment>>(commentsAsJson) ?? new List<Comment>();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found. Creating a new one.");
            return new List<Comment>(); 
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

    private async Task SaveAsync(List<Comment> comments)
    {
        try
        {
            string commentsAsJson = JsonSerializer.Serialize(comments);
            await File.WriteAllTextAsync(filePath, commentsAsJson);
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
