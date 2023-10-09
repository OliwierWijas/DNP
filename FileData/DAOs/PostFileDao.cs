using Application.DaoInterfaces;
using Shared;
using Shared.DTOs;

namespace FileData.DAOs;

public class PostFileDao : IPostDao
{
    private readonly FileContext context;

    public PostFileDao(FileContext context)
    {
        this.context = context;
    }
    
    public Task<Post> CreateAsync(Post post)
    {
        int postId = 1;
        if (context.Posts.Any())
        {
            postId = context.Posts.Max(p => p.Id);
            postId++;
        }

        post.Id = postId;
        
        context.Posts.Add(post);
        context.SaveChanges();
        
        return Task.FromResult(post);
    }

    public Task<Post?> GetByIdAsync(int postId)
    {
        Post? existing =
            context.Posts.FirstOrDefault(p => p.Id == postId);
        return Task.FromResult(existing);    
    }
    
    public Task<IEnumerable<Post>> GetAsync(PostSearchDto parameters)
    {
        IEnumerable<Post> posts = context.Posts.AsEnumerable();

        if (parameters.SubFormId is not null && parameters.SubFormId > 0)
        {
            posts = posts.Where(p => p.SubForm.Id == parameters.SubFormId);
        }

        if (!string.IsNullOrEmpty(parameters.Title))
        {
            posts = posts.Where(p => p.Title.Contains(parameters.Title, StringComparison.OrdinalIgnoreCase));
        }
        
        return Task.FromResult(posts);
    }

    public Task UpdateAsync(Post updated)
    {
        Post? existing =
            context.Posts.FirstOrDefault(p => p.Id == updated.Id);
        if (existing is null)
            throw new Exception($"Post with id {updated.Id} does not exist.");

        context.Posts.Remove(existing);
        context.Posts.Add(updated);
        context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? existing = context.Posts.FirstOrDefault(p => p.Id == id);
        if (existing is null)
            throw new Exception($"Post with id {id} does not exist.");

        context.Posts.Remove(existing);
        context.SaveChanges();

        return Task.CompletedTask;
    }
}