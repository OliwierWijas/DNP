namespace Shared.DTOs;

public class UserUpdateDto
{
    public string Username { get; }
    public string? NewUsername { get; set; }
    public string? Password { get; set; }

    public UserUpdateDto(string username)
    {
        this.Username = username;
    }
}