using Shared;
using Shared.DTOs;

namespace Application.LogicInterfaces;

public interface ICommentLogic
{
    Task<Comment> CreateAsync(CommentCreationDto dto);
    Task<IEnumerable<Comment>> GetAsync(int postId);
}