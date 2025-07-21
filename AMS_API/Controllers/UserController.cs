using BLL.Helpers;
using BLL.IService;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]

public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IAuthService _authService;
    private readonly ActionMapper actionMapper;

    public UserController(IUserService service, IAuthService authService)
    {
        _service = service;
        _authService = authService;
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
        if (response.IsSuccess && model.UserId == 0)
        {
            string? link = _authService.PrepareLink((User)response.Data!, HttpContext, true);
            string subject = EmailBodyHelper.UpsertUserSubject;
            string body = EmailBodyHelper.UpsertUserBody(link!);
            Response emailResponse = _authService.SendEmail(model.Email, body, subject);
            if (!emailResponse.IsSuccess)
            {
                emailResponse.Message = "User created but email sending failed.";
                return actionMapper.MapToActionResult(emailResponse);
            }
            else
                response.Message = "User created successfully and email sent.";
        }
        return actionMapper.MapToActionResult(response);
    }

    [HttpDelete("delete-user/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        Response response = await _service.DeleteUserAsync(id);
        return actionMapper.MapToActionResult(response);
    }

    [HttpGet("get-roles")]
    public async Task<IActionResult> GetRoles()
    {
        Response response = await _service.GetAllRolesAsync();
        return actionMapper.MapToActionResult(response);
    }
}
