using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class UserPermissionsController : ControllerBase
{
    private readonly IUserPermissionService _userPermissionService;

    public UserPermissionsController(IUserPermissionService userPermissionService)
    {
        _userPermissionService = userPermissionService;
    }

    [HttpPost]
    public async Task<ActionResult<UserPermission>> PostUserPermission(UserPermission userPermission)
    {
        var createdUserPermission = await _userPermissionService.CreateUserPermissionAsync(userPermission);
        return CreatedAtAction("GetUserPermission", new { id = createdUserPermission.UserPermissionId }, createdUserPermission);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserPermission>>> GetUserPermissions()
    {
        var userPermissions = await _userPermissionService.GetAllUserPermissionsAsync();
        return Ok(userPermissions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserPermission>> GetUserPermission(int id)
    {
        var userPermission = await _userPermissionService.GetUserPermissionByIdAsync(id);
        if (userPermission == null)
        {
            return NotFound();
        }
        return userPermission;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserPermission(int id, UserPermission userPermission)
    {
        if (id != userPermission.UserPermissionId)
        {
            return BadRequest();
        }

        try
        {
            await _userPermissionService.UpdateUserPermissionAsync(id, userPermission);
        }
        catch (Exception ex)
        {
            // Handle the exception appropriately
            return StatusCode(500, ex.Message);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserPermission(int id)
    {
        var success = await _userPermissionService.DeleteUserPermissionAsync(id);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
