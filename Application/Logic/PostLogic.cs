using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Shared;
using Shared.DTOs;

namespace Application.Logic;

public class PostLogic : IPostLogic
{
    private readonly IPostDao postDao;
    private readonly ISubFormDao subFormDao;

    public PostLogic(IPostDao postDao, ISubFormDao subFormDao)
    {
        this.postDao = postDao;
        this.subFormDao = subFormDao;
    }
    
    public async Task<Post> CreateAsync(PostBasicDto dto)
    {
        SubForm? subForm = await subFormDao.GetByIdAsync(dto.SubFormId);
        if (subForm is null)
            throw new Exception($"Sub-form with id {dto.SubFormId} was not found.");
        
        ValidatePost(dto.Title, dto.Body);
        Post post = new Post(subForm, dto.Title, dto.Body);
        Post created = await postDao.CreateAsync(post);
        return created;
    }

    public async Task<PostBasicDto> GetByIdAsync(int id)
    {
        Post? post = await postDao.GetByIdAsync(id);
        if (post is null)
            throw new Exception($"Post with id {id} was not found.");
        return new PostBasicDto(post.SubForm.Id, post.Title, post.Body);
    }

    public async Task<IEnumerable<Post>> GetAsync(PostSearchDto parameters)
    {
        return await postDao.GetAsync(parameters);
    }

    public async Task UpdateAsync(PostUpdateDto dto)
    {
        Post? existingPost = await postDao.GetByIdAsync(dto.Id);
        if (existingPost is null)
            throw new Exception($"Post with id {dto.Id} does not exist.");
        
        ValidatePost(dto.Title, dto.Body);

        Post updated = new Post(existingPost.SubForm, dto.Title, dto.Body)
        {
            Id = existingPost.Id
        };
        
        await postDao.UpdateAsync(updated);
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await postDao.GetByIdAsync(id);
        if (existing is null)
            throw new Exception($"Post with id {id} does not exist.");

        await postDao.DeleteAsync(id);
    }

    private static void ValidatePost(string? title, string? body)
    {
        if (string.IsNullOrEmpty(title))
            throw new Exception("Title of a post cannot be empty.");
        if (string.IsNullOrEmpty(body))
            throw new Exception("Body of a post cannot be empty.");
    }
}