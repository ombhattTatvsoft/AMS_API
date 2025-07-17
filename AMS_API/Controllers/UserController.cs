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
}
