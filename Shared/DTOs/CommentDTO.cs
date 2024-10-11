namespace DTOs;


public class CommentDTO
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int PostId { get; set; }
    public string PostTitle { get; set; }  // Optional: The title of the related post
    public int UserId { get; set; }
    public string UserName { get; set; }   // Optional: The username of the commenter
}

