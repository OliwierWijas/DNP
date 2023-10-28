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

    public async Task<IEnumerable<Comment>> GetAsync(int postId)
    {
        return await commentDao.GetAsync(postId);
    }

    private static void ValidateComment(string? text)
    {
        if (string.IsNullOrEmpty(text))
            throw new Exception("Comment cannot be empty.");
    }
}