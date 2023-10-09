using Shared;
using Shared.DTOs;

namespace HttpClient.ClientInterfaces;

public interface ICommentService
{
    Task CreateAsync(CommentCreationDto dto);
    Task<ICollection<Comment>> GetAsync(int postId);
}