using Shared;
using Shared.DTOs;

namespace Application.DaoInterfaces;

public interface ISubFormDao
{
    Task<SubForm> CreateAsync(SubForm subForm);
    Task<SubForm?> GetByIdAsync(int subFormId);
    Task<IEnumerable<SubForm>> GetAsync(SubFormBasicDto parameters);
    Task UpdateAsync(SubForm updated);
    Task DeleteAsync(int id);
}