namespace Shared.DTOs;

public class CommentCreationDto
{
    public int PostId { get; }
    public string Username { get; }
    public string Text { get;}

    public CommentCreationDto(int postId, string username, string text)
    {
        this.PostId = postId;
        this.Username = username;
        this.Text = text;
    }
}