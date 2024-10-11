namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }  // Ensure Post relationship exists
    public int UserId { get; set; }
    public User User { get; set; }  
    public Comment(string body, int postId, int userId)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
    }


    public Comment() { }
}