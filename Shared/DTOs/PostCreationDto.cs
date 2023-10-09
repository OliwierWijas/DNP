namespace Shared.DTOs;

public class PostCreationDto
{
    public int SubFormId { get; }
    public string Title { get;}
    public string Body { get; }

    public PostCreationDto(int subFormId, string title, string body)
    {
        this.SubFormId = subFormId;
        this.Title = title;
        this.Body = body;
    }
}