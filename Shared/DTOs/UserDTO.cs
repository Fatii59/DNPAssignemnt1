namespace DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public List<int> PostIds { get; set; }
    public List<int> CommentIds { get; set; }
}