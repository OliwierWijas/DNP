using Shared;
using Shared.DTOs;

namespace Application.DaoInterfaces;

public interface ICommentDao
{
    Task<Comment> CreateAsync(Comment comment);
    Task<Comment?> GetByIdAsync(int id);
    Task<IEnumerable<Comment>> GetAsync(CommentSearchDto parameters);
    Task UpdateAsync(Comment updated);
    Task DeleteAsync(int id);
}