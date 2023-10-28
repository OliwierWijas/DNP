using Application.DaoInterfaces;
using Shared;

namespace FileData.DAOs;

public class CommentFileDao : ICommentDao
{
    private readonly FileContext context;

    public CommentFileDao(FileContext context)
    {
        this.context = context;
    }

    public Task<Comment> CreateAsync(Comment comment)
    {
        int commentId = 1;
        if (context.Comments.Any())
        {
            commentId = context.Comments.Max(c => c.Id);
            commentId++;
        }

        comment.Id = commentId;

        context.Comments.Add(comment);
        context.SaveChanges();

        return Task.FromResult(comment);
    }

    public Task<IEnumerable<Comment>> GetAsync(int postId)
    {
        IEnumerable<Comment> comments = context.Comments.AsEnumerable();

        comments = comments.Where(c => c.Post.Id == postId);

        return Task.FromResult(comments);
    }
}