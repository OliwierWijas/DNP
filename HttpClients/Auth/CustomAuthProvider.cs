using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using HttpClient.ClientInterfaces;

namespace HttpClient.Auth;

public class CustomAuthProvider : AuthenticationStateProvider
{
    private readonly IUserService userService;

    public CustomAuthProvider(IUserService userService)
    {
        this.userService = userService;
        this.userService.OnAuthStateChanged += AuthStateChanged;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsPrincipal principal = await userService.GetAuthAsync();

        return new AuthenticationState(principal);
    }

    private void AuthStateChanged(ClaimsPrincipal principal)
    {
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(principal)));
    }
}