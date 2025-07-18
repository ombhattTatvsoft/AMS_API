using BLL.Helpers;
using BLL.IService;
using Entity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "Admin")]

public class UserController : ControllerBase
{
    private readonly IUserService _service;

    private readonly ActionMapper actionMapper;

    public UserController(IUserService service)
    {
        _service = service;
        actionMapper = new ActionMapper();
    }

    [HttpGet("get-users")]
    public async Task<IActionResult> GetUsers()
    {
        Response response = await _service.GetAllUsersAsync();
        return actionMapper.MapToActionResult(response);
    }

    [HttpGet("get-user/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        Response response = await _service.GetUserAsync(id);
        return actionMapper.MapToActionResult(response);
    }

    [HttpPost("save-user")]
    public async Task<IActionResult> SaveUser(UserDTO model)
    {
        if (model.RoleId == 2)
        {
            ModelState.Remove("ManagerId");
        }
        Response response = await _service.SaveUserAsync(model);
        return actionMapper.MapToActionResult(response);
    }

    [HttpGet("get-roles")]
    public async Task<IActionResult> GetRoles()
    {
        Response response = await _service.GetAllRolesAsync();
        return actionMapper.MapToActionResult(response);
    }
}
