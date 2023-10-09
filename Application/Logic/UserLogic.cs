using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Shared;
using Shared.DTOs;

namespace Application.Logic;

public class UserLogic : IUserLogic
{
    private readonly IUserDao userDao;

    public UserLogic(IUserDao userDao)
    {
        this.userDao = userDao;
    }
    
    public async Task<User> CreateAsync(User user)
    {
        User? exisitng = await userDao.GetByUsernameAsync(user.Username);
        if (exisitng is not null)
            throw new Exception("Username already taken.");

        ValidateUsername(user.Username);
        ValidatePassword(user.Password);

        User created = await userDao.CreateAsync(user);
        return created;
    }

    public async Task<User> LoginAsync(User user)
    {
        User? existing = await userDao.GetByUsernameAsync(user.Username);
        if (existing is null)
            throw new Exception($"User with username {user.Username} was not found.");
        else if (!existing.Password.Equals(user.Password))
            throw new Exception("Password mismatch.");
        return existing;
    }

    public async Task UpdateAsync(UserUpdateDto dto)
    {
        User? existingUser = await userDao.GetByUsernameAsync(dto.Username);
        if (existingUser is null)
            throw new Exception("User does not exist.");
        
        ValidateUsername(dto.NewUsername);
        ValidatePassword(dto.Password);

        User updated = new User(dto.NewUsername, dto.Password);

        await userDao.UpdateAsync(existingUser.Username, updated);
    }

    public async Task DeleteAsync(string username)
    {
        User? existing = await userDao.GetByUsernameAsync(username);
        if (existing is null)
            throw new Exception($"User with username {username} was not found.");

        await userDao.DeleteAsync(username);
    }

    private static void ValidateUsername(string? username)
    {
        if (string.IsNullOrEmpty(username))
            throw new Exception("Username cannot be empty.");
        if (username.Length < 4)
            throw new Exception("Username must have at least 4 characters.");
        if (username.Length > 17)
            throw new Exception("Username must have less than 17 characters.");
    }

    private static void ValidatePassword(string? password)
    {
        if (string.IsNullOrEmpty(password))
            throw new Exception("Password cannot be empty.");
        if (password.Length < 8)
            throw new Exception("Password must have at least 8 characters.");
        if (password.Length > 21)
            throw new Exception("Password must have less than 21 characters.");

        bool lowercase = false;
        bool uppercase = false;
        bool digit = false;
        char[] chars = password.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            char ch = chars[i];
            lowercase = lowercase || Char.IsLower(ch);
            uppercase = uppercase || Char.IsUpper(ch);
            digit = digit || Char.IsDigit(ch);
        }

        if (!lowercase)
            throw new Exception("Password must contain at least one lowercase character.");
        if (!uppercase)
            throw new Exception("Password must contain at least one uppercase character.");
        if (!digit)
            throw new Exception("Password must contain at least one digit.");
    }
}