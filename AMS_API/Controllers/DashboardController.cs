using System.Net;
using BLL.Constants;
using BLL.Helpers;
using BLL.IService;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace AMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public DashboardController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            User? user = await _authService.AuthenticateUserAsync(model.Email, model.CurrentPassword);
            if (user != null)
            {
                await _userService.UpdatePasswordAsync(user, model.NewPassword);
                return this.MapToActionResult(new(true,Constant.PASSWORD_CHANGE_SUCCESS,null,HttpStatusCode.OK));
            }
            return this.MapToActionResult(new Response(false, Constant.PASSWORD_CHANGE_FAIL, null, HttpStatusCode.BadRequest));
        }
    }
}