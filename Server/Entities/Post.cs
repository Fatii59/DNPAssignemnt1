namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }

    // Foreign key for User
    public int UserId { get; set; }
    public User User { get; set; }

    // Navigation property for related comments
    public List<Comment> Comments { get; set; } = new();

    // Timestamp for post creation
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Parameterless constructor for EFC
    private Post() { }

    public Post(string title, string body, int userId)
    {
        Title = title;
        Body = body;
        UserId = userId;
    }
}