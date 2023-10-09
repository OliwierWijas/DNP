namespace Shared.DTOs;

public class CommentUpdateDto
{
    public int Id { get; }
    public string? Text { get; set; }

    public CommentUpdateDto(int id)
    {
        this.Id = id;
    }
}