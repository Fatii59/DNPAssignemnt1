namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<Comment> Comments { get; set; }
    
    // Ensure `CreatedDate` is set only once when a new post is created
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Post(string title, string body, int userId)
    {
        Title = title;
        Body = body;
        UserId = userId;
        Comments = new List<Comment>();
    }

    public Post() // Default constructor required for deserialization
    {
        Comments = new List<Comment>();
    }
}