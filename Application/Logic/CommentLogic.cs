using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Shared;
using Shared.DTOs;

namespace Application.Logic;

public class CommentLogic : ICommentLogic
{
    private readonly ICommentDao commentDao;
    private readonly IPostDao postDao;
    private readonly IUserDao userDao;

    public CommentLogic(ICommentDao commentDao, IPostDao postDao, IUserDao userDao)
    {
        this.commentDao = commentDao;
        this.postDao = postDao;
        this.userDao = userDao;
    }
    
    public async Task<Comment> CreateAsync(CommentCreationDto dto)
    {
        User? user = await userDao.GetByUsernameAsync(dto.Username);
        if (user is null)
            throw new Exception($"User with username {dto.Username} was not found.");
        Post? post = await postDao.GetByIdAsync(dto.PostId);
        if (post is null)
            throw new Exception($"Post with id {dto.PostId} was not found.");
        
        ValidateComment(dto.Text);
        Comment comment = new Comment(post, user, dto.Text);
        Comment created = await commentDao.CreateAsync(comment);
        return created;
    }

    public async Task<CommentCreationDto> GetByIdAsync(int id)
    {
        Comment? comment = await commentDao.GetByIdAsync(id);
        if (comment is null)
            throw new Exception($"Comment with id {id} was not found.");
        return new CommentCreationDto(comment.Post.Id, comment.User.Username, comment.Text);
    }

    public async Task<IEnumerable<Comment>> GetAsync(CommentSearchDto parameters)
    {
        return await commentDao.GetAsync(parameters);
    }

    public async Task UpdateAsync(CommentUpdateDto dto)
    {
        Comment? existing = await commentDao.GetByIdAsync(dto.Id);
        if (existing is null)
            throw new Exception($"Comment with id {dto.Id} was not found.");
        
        ValidateComment(dto.Text);

        Comment updated = new Comment(existing.Post, existing.User, dto.Text)
        {
            Id = existing.Id
        };

        await commentDao.UpdateAsync(updated);
    }

    public async Task DeleteAsync(int id)
    {
        Comment? existing = await commentDao.GetByIdAsync(id);
        if (existing is null)
            throw new Exception($"Comment with id {id} was not found.");

        await commentDao.DeleteAsync(id);
    }

    private static void ValidateComment(string? text)
    {
        if (string.IsNullOrEmpty(text))
            throw new Exception("Comment cannot be empty.");
    }
}