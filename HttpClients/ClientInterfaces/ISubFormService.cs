using Shared;
using Shared.DTOs;

namespace HttpClient.ClientInterfaces;

public interface ISubFormService
{
    Task CreateAsync(SubFormBasicDto dto);
    Task<ICollection<SubForm>> GetAsync(string username, string? name);
    Task<SubFormBasicDto> GetByIdAsync(int id);
    Task UpdateAsync(SubFormUpdateDto dto);
    Task DeleteAsync(int id);
}