using Application.DaoInterfaces;
using Shared;

namespace FileData.DAOs;

public class UserFileDao : IUserDao
{
    private readonly FileContext context;

    public UserFileDao(FileContext context)
    {
        this.context = context;
    }
    
    public Task<User> CreateAsync(User user)
    {
        context.Users.Add(user);
        context.SaveChanges();

        return Task.FromResult(user);
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        User? existing =
            context.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(existing);
    }

    public Task UpdateAsync(string username, User updated)
    {
        User? existing =
            context.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        if (existing is null)
            throw new Exception($"User with username {username} does not exist.");
        
        User? usernameCheck =
            context.Users.FirstOrDefault(u => u.Username.Equals(updated.Username, StringComparison.OrdinalIgnoreCase));
        if (usernameCheck is not null)
            throw new Exception("Username already taken.");

        context.Users.Remove(existing);
        context.Users.Add(updated);
        context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string username)
    {
        User? existing =
            context.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        if (existing is null)
            throw new Exception($"User with username {username} does not exist.");

        context.Users.Remove(existing);
        context.SaveChanges();

        return Task.CompletedTask;
    }
}