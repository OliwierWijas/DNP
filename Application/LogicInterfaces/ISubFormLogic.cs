using Shared;
using Shared.DTOs;

namespace Application.LogicInterfaces;

public interface ISubFormLogic
{
    Task<SubForm> CreateAsync(SubFormBasicDto dto);
    Task<SubFormBasicDto> GetByIdAsync(int id);
    Task<IEnumerable<SubForm>> GetAsync(SubFormBasicDto parameters);
    Task UpdateAsync(SubFormUpdateDto dto);
    Task DeleteAsync(int id);
}