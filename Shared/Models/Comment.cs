using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared;

public class Comment
{
    [Key]
    public int Id { get; set; }
    public Post Post { get; set; }
    public User User { get; set; }
    public string Text { get; set; }

    public Comment(Post post, User user, string text)
    {
        this.Post = post;
        this.User = user;
        this.Text = text;
    }
    
    private Comment(){}
}