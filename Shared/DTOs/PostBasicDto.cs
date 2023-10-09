namespace Shared.DTOs;

public class PostBasicDto
{
    public int SubFormId { get; }
    public string Title { get; set; }
    public string Body { get; set; }

    public PostBasicDto(int subFormId, string title, string body)
    {
        this.SubFormId = subFormId;
        this.Title = title;
        this.Body = body;
    }
}