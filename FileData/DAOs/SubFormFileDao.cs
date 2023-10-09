using Application.DaoInterfaces;
using Shared;
using Shared.DTOs;

namespace FileData.DAOs;

public class SubFormFileDao : ISubFormDao
{
    private readonly FileContext context;

    public SubFormFileDao(FileContext context)
    {
        this.context = context;
    }
    public Task<SubForm> CreateAsync(SubForm subForm)
    {
        int subFormId = 1;
        if (context.SubForms.Any())
        {
            subFormId = context.SubForms.Max(s => s.Id);
            subFormId++;
        }

        subForm.Id = subFormId;
        
        context.SubForms.Add(subForm);
        context.SaveChanges();
        
        return Task.FromResult(subForm);
    }

    public Task<SubForm?> GetByIdAsync(int subFormId)
    {
        SubForm? existing =
            context.SubForms.FirstOrDefault(s => s.Id == subFormId);
        return Task.FromResult(existing);
    }

    public Task<IEnumerable<SubForm>> GetAsync(SubFormBasicDto parameters)
    {
        IEnumerable<SubForm> subForms = context.SubForms.AsEnumerable();

        if (!string.IsNullOrEmpty(parameters.Username))
        {
            subForms = subForms.Where(s => s.User.Username.Equals(parameters.Username,
                StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(parameters.Name))
        {
            subForms = subForms.Where(s => s.Name.Contains(parameters.Name, StringComparison.OrdinalIgnoreCase));
        }
        
        return Task.FromResult(subForms);
    }

    public Task UpdateAsync(SubForm updated)
    {
        SubForm? existing =
            context.SubForms.FirstOrDefault(s => s.Id == updated.Id);
        if (existing is null)
            throw new Exception($"Sub form with id {updated.Id} does not exist.");

        context.SubForms.Remove(existing);
        context.SubForms.Add(updated);
        context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        SubForm? existing = context.SubForms.FirstOrDefault(s => s.Id == id);
        if (existing is null)
            throw new Exception($"Sub form with id {id} does not exist.");

        context.SubForms.Remove(existing);
        context.SaveChanges();
        
        return Task.CompletedTask;
    }
}