using Application.DaoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared;

namespace EfcDataAccess.DAOs;

public class UserEfcDao : IUserDao
{
    private readonly Context context;

    public UserEfcDao(Context context)
    {
        this.context = context;
    }
    
    public async Task<User> CreateAsync(User user)
    {
        EntityEntry<User> newUser = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return newUser.Entity;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        User? existing =
            await context.Users.FirstOrDefaultAsync(user => user.Username.ToLower().Equals(username.ToLower()));
        return existing;
    }
}