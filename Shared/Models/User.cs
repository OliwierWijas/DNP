using System.ComponentModel.DataAnnotations;

namespace Shared;

public class User
{
    [Key]
    public string Username { get; set; }
    public string Password { get;  set; }

    public User(string username, string password)
    {
        this.Username = username;
        this.Password = password;
    }
    
    private User(){}
}