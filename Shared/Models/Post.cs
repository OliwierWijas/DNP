namespace Shared;

public class Post
{
    public int Id { get; set; }
    public SubForm SubForm { get; }
    public string Title { get;}
    public string Body { get; }

    public Post(SubForm subForm, string title, string body)
    {
        this.SubForm = subForm;
        this.Title = title;
        this.Body = body;
    }
}