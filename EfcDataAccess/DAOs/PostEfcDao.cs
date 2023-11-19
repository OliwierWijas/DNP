using Application.DaoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared;
using Shared.DTOs;

namespace EfcDataAccess.DAOs;

public class PostEfcDao : IPostDao
{
    private readonly Context context;

    public PostEfcDao(Context context)
    {
        this.context = context;
    }
    
    public async Task<Post> CreateAsync(Post post)
    {
        EntityEntry<Post> newPost = await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();
        return newPost.Entity;    
    }

    public async Task<Post?> GetByIdAsync(int postId)
    {
        Post? post = await context.Posts.Include(post => post.SubForm).ThenInclude(subForm => subForm.User).SingleOrDefaultAsync(post => post.Id == postId);
        return post;
    }

    public async Task<IEnumerable<Post>> GetAsync(PostSearchDto parameters)
    {
        IQueryable<Post> postsQuery = context.Posts.AsQueryable();
        if (parameters.SubFormId is not null)
        {
            postsQuery =
                postsQuery.Where(post => post.SubForm.Id == parameters.SubFormId);
        }

        if (parameters.Title is not null)
        {
            postsQuery = postsQuery.Where(post => post.Title.ToLower().Contains(parameters.Title.ToLower()));
        }

        IEnumerable<Post> result = await postsQuery.ToListAsync();
        return result;    
    }

    public async Task UpdateAsync(Post updated)
    {
        context.ChangeTracker.Clear();
        context.Posts.Update(updated);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await GetByIdAsync(id);
        if(existing is null)
        {
            throw new Exception($"Post with id {id} was not found.");
        }

        context.Posts.Remove(existing);
        await context.SaveChangesAsync();
    }
}