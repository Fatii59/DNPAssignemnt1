namespace InMemoryRepositories;


using Entities;
using RepostitoryContracts;

public class CommentInMemoryRepository : ICommentRepository

{
    private List<Comment> comments = new List<Comment>();

    public CommentInMemoryRepository()
    {
// dummy data
        comments = new List<Comment>
        {
            new Comment
            {
                Id = 1,
                PostId = 1, 
                Body = "This is a great first post!",
                UserId = 3,

            },
            new Comment
            {
                Id = 2,
                PostId = 2, 
                Body = "Looking forward to more content!",
                UserId = 1, 

            }
        };
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any()   ? comments.Max(c => c.Id) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment == null)
        {
            throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
        }
        
        comments.Remove(existingComment);
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove=comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove == null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }
        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = comments.SingleOrDefault(c => c.Id == id);
        if (comment == null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }
        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
    
    //Dummy data??
}