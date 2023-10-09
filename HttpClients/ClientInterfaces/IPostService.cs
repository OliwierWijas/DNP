using Shared;
using Shared.DTOs;

namespace HttpClient.ClientInterfaces;

public interface IPostService
{
    Task CreateAsync(PostCreationDto dto);
    Task<ICollection<Post>> GetAsync(int? subFormId, string? title);
    Task<PostBasicDto> GetByIdAsync(int id);
    Task UpdateAsync(PostUpdateDto dto);
    Task DeleteAsync(int id);
}