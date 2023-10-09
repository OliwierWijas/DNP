using Application.DaoInterfaces;
using Shared;
using Shared.DTOs;

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

    public Task<Comment?> GetByIdAsync(int id)
    {
        Comment? existing =
            context.Comments.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(existing);    
    }

    public Task<IEnumerable<Comment>> GetAsync(CommentSearchDto parameters)
    {
        IEnumerable<Comment> comments = context.Comments.AsEnumerable();

        if (parameters.PostId is not null && parameters.PostId > 0)
        {
            comments = comments.Where(c => c.Post.Id == parameters.PostId);
        }

        if (!string.IsNullOrEmpty(parameters.Username))
        {
            comments = comments.Where(c => c.User.Username.Equals(parameters.Username, StringComparison.OrdinalIgnoreCase));
        }
        
        return Task.FromResult(comments);
    }

    public Task UpdateAsync(Comment updated)
    {
        Comment? existing =
            context.Comments.FirstOrDefault(c => c.Id == updated.Id);
        if (existing is null)
            throw new Exception($"Comment with id {updated.Id} does not exist.");

        context.Comments.Remove(existing);
        context.Comments.Add(updated);
        context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? existing = context.Comments.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            throw new Exception($"Comment with id {id} does not exist.");

        context.Comments.Remove(existing);
        context.SaveChanges();

        return Task.CompletedTask;
    }
}