namespace Shared.DTOs;

public class PostUpdateDto
{
    public int Id { get; }
    public string? Title { get; set; }
    public string? Body { get; set; }

    public PostUpdateDto(int id)
    {
        this.Id = id;
    }
}