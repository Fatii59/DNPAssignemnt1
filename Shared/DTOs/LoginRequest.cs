namespace DTOs
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        // Constructor that accepts UserName and Password as parameters
        public LoginRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        // Parameterless constructor for serialization and deserialization
        public LoginRequest() { }
    }
}