using System.Security.Claims;
using Shared;

namespace HttpClient.ClientInterfaces;

public interface IUserService
{
    Task LoginAsync(User user);
    Task LogoutAsync();
    public Task RegisterAsync(User user);
    Task<ClaimsPrincipal> GetAuthAsync();
    
    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; }
}