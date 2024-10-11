namespace DTOs;

public class CreatePostDTO
{
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }  // The user creating the post
}