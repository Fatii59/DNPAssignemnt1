namespace DTOs;

public class CreateCommentDTO
{
    public string Body { get; set; }
    public int PostId { get; set; }  // The post to which the comment belongs
    public int UserId { get; set; }  // The user who made the comment
}