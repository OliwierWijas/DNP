using Shared;
using Shared.DTOs;

namespace Application.LogicInterfaces;

public interface IUserLogic
{
    Task<User> CreateAsync(User user);
    Task<User> LoginAsync(User user);
    Task UpdateAsync(UserUpdateDto dto);
    Task DeleteAsync(string username);
}