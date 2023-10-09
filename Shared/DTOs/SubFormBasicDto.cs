namespace Shared.DTOs;

public class SubFormBasicDto
{
    public string? Username { get; }
    public string? Name { get; }

    public SubFormBasicDto(string? username, string? name)
    {
        this.Username = username;
        this.Name = name;
    }
}