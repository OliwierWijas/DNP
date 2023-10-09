using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Shared;
using Shared.DTOs;

namespace Application.Logic;

public class SubFormLogic : ISubFormLogic
{
    private readonly ISubFormDao subFormDao;
    private readonly IUserDao userDao;

    public SubFormLogic(ISubFormDao subFormDao, IUserDao userDao)
    {
        this.subFormDao = subFormDao;
        this.userDao = userDao;
    }
    
    public async Task<SubForm> CreateAsync(SubFormBasicDto dto)
    {
        User? user = await userDao.GetByUsernameAsync(dto.Username);
        if (user is null)
            throw new Exception($"User with username {dto.Username} was not found.");
        
        ValidateSubForm(dto.Name);
        SubForm subForm = new SubForm(user, dto.Name);
        SubForm created = await subFormDao.CreateAsync(subForm);
        return created;
    }

    public async Task<SubFormBasicDto> GetByIdAsync(int id)
    {
        SubForm? subForm = await subFormDao.GetByIdAsync(id);
        if (subForm is null)
            throw new Exception($"Sub-form with id {id} was not found.");
        return new SubFormBasicDto(subForm.User.Username, subForm.Name);
    }

    public async Task<IEnumerable<SubForm>> GetAsync(SubFormBasicDto parameters)
    {
        return await subFormDao.GetAsync(parameters);
    }

    public async Task UpdateAsync(SubFormUpdateDto dto)
    {
        SubForm? existingSubForm = await subFormDao.GetByIdAsync(dto.Id);
        if (existingSubForm is null)
            throw new Exception($"Sub form with id {dto.Id} does not exist.");
        
        ValidateSubForm(dto.Name);

        SubForm updated = new SubForm(existingSubForm.User, dto.Name)
        {
            Id = existingSubForm.Id
        };
        
        await subFormDao.UpdateAsync(updated);
    }

    public async Task DeleteAsync(int id)
    {
        SubForm? existing = await subFormDao.GetByIdAsync(id);
        if (existing is null)
            throw new Exception($"Sub form with id {id} was not found.");

        await subFormDao.DeleteAsync(id);
    }

    private static void ValidateSubForm(string? name)
    {
        if (string.IsNullOrEmpty(name))
            throw new Exception("Name of a sub-form cannot be empty.");
    }
}