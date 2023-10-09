namespace Shared.DTOs;

public class PostSearchDto
{
    public int? SubFormId { get; }
    public string? Title { get;}

    public PostSearchDto(int? subFormId, string? title)
    {
        this.SubFormId = subFormId;
        this.Title = title; 
    }
}