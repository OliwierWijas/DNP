using Shared;
using Shared.DTOs;

namespace Application.DaoInterfaces;

public interface IPostDao
{
    Task<Post> CreateAsync(Post post);
    Task<Post?> GetByIdAsync(int postId);
    Task<IEnumerable<Post>> GetAsync(PostSearchDto parameters);
    Task UpdateAsync(Post updated);
    Task DeleteAsync(int id);
}