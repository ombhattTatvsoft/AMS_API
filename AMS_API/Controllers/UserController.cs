using BLL.Constants;
using BLL.Helpers;
using BLL.IService;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "Admin")]

public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IAuthService _authService;

    public UserController(IUserService service, IAuthService authService)
    {
        _service = service;
        _authService = authService;
    }

    [HttpGet("get-users")]
    public async Task<IActionResult> GetUsers()
    {
        Response response = await _service.GetAllUsersAsync();
        return this.MapToActionResult(response);
    }

    [HttpGet("get-user/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        Response response = await _service.GetUserAsync(id);
        return this.MapToActionResult(response);
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
                emailResponse.Message = Constant.SAVE_USER_EMAIL_FAIL;
                return this.MapToActionResult(emailResponse);
            }
            else
                response.Message = Constant.SAVE_USER_SUCCESS;
        }
        return this.MapToActionResult(response);
    }

    [HttpDelete("delete-user/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        Response response = await _service.DeleteUserAsync(id);
        return this.MapToActionResult(response);
    }

    [HttpGet("get-roles")]
    public async Task<IActionResult> GetRoles()
    {
        Response response = await _service.GetAllRolesAsync();
        return this.MapToActionResult(response);
    }
}
