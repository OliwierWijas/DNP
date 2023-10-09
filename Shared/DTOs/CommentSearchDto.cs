namespace Shared.DTOs;

public class CommentSearchDto
{
    public int? PostId { get; }
    public string? Username { get; }

    public CommentSearchDto(int? postId, string? username)
    {
        this.PostId = postId;
        this.Username = username;
    }
}