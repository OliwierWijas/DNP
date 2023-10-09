namespace Shared.DTOs;

public class SubFormUpdateDto
{
    public int Id { get; }
    public string? Name { get; set; }

    public SubFormUpdateDto(int id)
    {
        this.Id = id;
    }
}