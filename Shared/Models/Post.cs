using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared;

public class Post
{
    [Key]
    public int Id { get; set; }
    public SubForm SubForm { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }

    public Post(SubForm subForm, string title, string body)
    {
        this.SubForm = subForm;
        this.Title = title;
        this.Body = body;
    }
    
    private Post(){}
}