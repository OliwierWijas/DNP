using System.Security.Claims;
using Application.LogicInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SubFormsController : ControllerBase
{
    private readonly ISubFormLogic subFormLogic;

    public SubFormsController(ISubFormLogic subFormLogic)
    {
        this.subFormLogic = subFormLogic;
    }

    [HttpPost]
    public async Task<ActionResult<SubForm>> CreateAsync(SubFormBasicDto dto)
    {
        try
        {
            SubForm subForm = await subFormLogic.CreateAsync(dto);
            return Created($"/subforms/{subForm.Id}", subForm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SubFormBasicDto>> GetByIdAsync([FromRoute] int id)
    {
        try
        {
            SubFormBasicDto result = await subFormLogic.GetByIdAsync(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<SubForm>> GetAsync([FromQuery] string? username, [FromQuery] string? name)
    {
        try
        {
            SubFormBasicDto parameters = new SubFormBasicDto(username, name);
            var result = await subFormLogic.GetAsync(parameters);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateAsync(SubFormUpdateDto dto)
    {
        try
        {
            await subFormLogic.UpdateAsync(dto);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await subFormLogic.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}