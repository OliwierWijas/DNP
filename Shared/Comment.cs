namespace Shared;

public class Comment
{
    public int Id { get; set; }
    public Post Post { get; }
    public User User { get; }
    public string Text { get; }

    public Comment(Post post, User user, string text)
    {
        this.Post = post;
        this.User = user;
        this.Text = text;
    }
}