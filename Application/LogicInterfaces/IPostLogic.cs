using Shared;
using Shared.DTOs;

namespace Application.LogicInterfaces;

public interface IPostLogic
{
    Task<Post> CreateAsync(PostCreationDto dto);
    Task<PostBasicDto> GetByIdAsync(int id);
    Task<IEnumerable<Post>> GetAsync(PostSearchDto parameters);
    Task UpdateAsync(PostUpdateDto dto);
    Task DeleteAsync(int id);
}