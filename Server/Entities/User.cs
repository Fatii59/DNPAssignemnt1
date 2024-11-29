namespace Entities;


public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    // Navigation properties
    public List<Post> Posts { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();

    // Parameterless constructor for EFC
    private User() { }

    public User(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}