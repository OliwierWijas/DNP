using Application.DaoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared;

namespace EfcDataAccess.DAOs;

public class CommentEfcDao : ICommentDao
{
    private readonly Context context;

    public CommentEfcDao(Context context)
    {
        this.context = context;
    }
    
    public async Task<Comment> CreateAsync(Comment comment)
    {
        EntityEntry<Comment> newComment = await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();
        return newComment.Entity;
    }

    public async Task<IEnumerable<Comment>> GetAsync(int postId)
    {
        return await context.Comments.Include(comment => comment.User).Include(c => c.Post).Where(c => c.Post.Id == postId).ToListAsync();
    }
}