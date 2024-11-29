namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }

    // Foreign key for Post
    public int PostId { get; set; }
    public Post Post { get; set; }

    // Foreign key for User
    public int UserId { get; set; }
    public User User { get; set; }

    // Parameterless constructor for EFC
    private Comment() { }

    public Comment(string body, int postId, int userId)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
    }
}