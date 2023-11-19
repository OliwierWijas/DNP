using Application.DaoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared;
using Shared.DTOs;

namespace EfcDataAccess.DAOs;

public class SubFormEfcDao : ISubFormDao
{
    private readonly Context context;

    public SubFormEfcDao(Context context)
    {
        this.context = context;
    }
    
    public async Task<SubForm> CreateAsync(SubForm subForm)
    {
        EntityEntry<SubForm> newSubForm = await context.SubForms.AddAsync(subForm);
        await context.SaveChangesAsync();
        return newSubForm.Entity;
    }

    public async Task<SubForm?> GetByIdAsync(int subFormId)
    {
        SubForm? subForm = await context.SubForms.Include(subForm => subForm.User).SingleOrDefaultAsync(subForm => subForm.Id == subFormId);
        return subForm;
    }

    public async Task<IEnumerable<SubForm>> GetAsync(SubFormBasicDto parameters)
    {
        IQueryable<SubForm> subFormsQuery = context.SubForms.AsQueryable();
        if (parameters.Username is not null)
        {
            subFormsQuery =
                subFormsQuery.Where(subForm => subForm.User.Username.ToLower().Contains(parameters.Username.ToLower()));
        }

        if (parameters.Name is not null)
        {
            subFormsQuery = subFormsQuery.Where(subForm => subForm.Name.ToLower().Contains(parameters.Name.ToLower()));
        }

        IEnumerable<SubForm> result = await subFormsQuery.ToListAsync();
        return result;
    }

    public async Task UpdateAsync(SubForm updated)
    {
        context.ChangeTracker.Clear(); 
        context.SubForms.Update(updated);
        await context.SaveChangesAsync(); 
    }

    public async Task DeleteAsync(int id)
    {
        SubForm? existing = await GetByIdAsync(id);
        if(existing is null)
        {
            throw new Exception($"Sub form with id {id} was not found.");
        }

        context.SubForms.Remove(existing);
        await context.SaveChangesAsync();
    }
}