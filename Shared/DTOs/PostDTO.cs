namespace DTOs;


public class PostDTO
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public List<int>? CommentIds { get; set; }  // Optional: Include comment IDs
    public DateTime CreatedDate { get; set; } // Add CreatedDate
    public int CommentCount { get; set; } 
}
