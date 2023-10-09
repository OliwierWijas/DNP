using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.LogicInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Shared.DTOs;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserLogic userLogic;
    private readonly IConfiguration config;
    
    public UsersController(IConfiguration config, IUserLogic userLogic)
    {
        this.userLogic = userLogic;
        this.config = config;
    }

    private List<Claim> GenerateClaims(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, config["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
        };
        return claims.ToList();
    }
    
    private string GenerateJwt(User user)
    {
        List<Claim> claims = GenerateClaims(user);
    
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
    
        JwtHeader header = new JwtHeader(signIn);
    
        JwtPayload payload = new JwtPayload(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims, 
            null,
            DateTime.UtcNow.AddMinutes(60));
    
        JwtSecurityToken token = new JwtSecurityToken(header, payload);
    
        string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return serializedToken;
    }
    
    [HttpPost, Route("signin"), AllowAnonymous]
    public async Task<ActionResult<User>> SignInAsync([FromBody] User user)
    {
        try
        {
            User created = await userLogic.CreateAsync(user);
            string token = GenerateJwt(created);
            return Created($"/users/{created.Username}", token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost, Route("login"), AllowAnonymous]
    public async Task<ActionResult<User>> LoginAsync([FromBody] User user)
    {
        try
        {
            User result = await userLogic.LoginAsync(user);
            string token = GenerateJwt(result);
            return Ok(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch, Authorize]
    public async Task<ActionResult> UpdateAsync(UserUpdateDto dto)
    {
        try
        {
            string check = UsernameClaimValidation(dto.Username);
            if (!string.IsNullOrEmpty(check))
                return StatusCode(403, check);
            await userLogic.UpdateAsync(dto);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{username}"), Authorize]
    public async Task<ActionResult> DeleteAsync([FromRoute] string username)
    {
        try
        {
            string check = UsernameClaimValidation(username);
            if (!string.IsNullOrEmpty(check))
                return StatusCode(403, check);
            await userLogic.DeleteAsync(username);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    private string UsernameClaimValidation(string username)
    {
        string message = "";
        Claim? usernameClaim = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Name));
        if (usernameClaim == null)
            message = "Not Authorized.";
        if (!usernameClaim!.Value.Equals(username))
            message = "Cannot delete different users.";
        return message;
    }
}