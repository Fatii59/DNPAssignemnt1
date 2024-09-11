namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<Comment> Comments { get; set; }


    public Post(string title, string body, int userId)
    {
        Title = title;
        Body = body;
        UserId = userId;
        Comments = new List<Comment>();
    }

    
    public Post()
    {
        Comments = new List<Comment>();
    }
}