using Shared;
using Shared.DTOs;

namespace Application.LogicInterfaces;

public interface ICommentLogic
{
    Task<Comment> CreateAsync(CommentCreationDto dto);
    Task<CommentCreationDto> GetByIdAsync(int id);
    Task<IEnumerable<Comment>> GetAsync(CommentSearchDto parameters);
    Task UpdateAsync(CommentUpdateDto dto);
    Task DeleteAsync(int id);
}