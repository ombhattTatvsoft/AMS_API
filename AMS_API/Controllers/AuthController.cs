using BLL.Constants;
using BLL.Helpers;
using BLL.IService;
using Entity.DTOs;
using Entity.Models;
using Entity.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        private readonly IUserService _userService;

        public AuthController(IAuthService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO user)
        {
            User? userFound = await _service.AuthenticateUserAsync(user.Email.Trim(), user.Password);
            Response response = new(false);
            if (userFound == null)
            {
                response.Message = Constant.WRONG_CRED;
                return Unauthorized(response);
            }
            string? token = _service.CreateJwtToken(userFound,user.Rememberme);
            if (token == null)
            {
                response.Message = Constant.LOGIN_FAIL;
                return StatusCode(500, response);
            }
            response.IsSuccess = true;
            response.Message = Constant.LOGIN_SUCCESS;
            response.Data = new
            {
                token,
                user = new
                {
                    id = userFound.UserId,
                    name = userFound.Name,
                    role = userFound.Role.RoleName,
                    email = userFound.Email
                }
            };
            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO user)
        {
            User? userFound = await _userService.FindUserByEmailAsync(user.Email.Trim());
            Response response = new(false);
            if (userFound != null)
            {
                string? link = _service.PrepareLink(userFound, HttpContext);
                string subject = EmailBodyHelper.ForgotPasswordSubject;
                string body = EmailBodyHelper.ForgotPasswordBody(link!);

                response = _service.SendEmail(userFound.Email, body, subject);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            else
            {
                response.Message = Constant.USER_NOT_FOUND;
                return NotFound(response);
            }
        }

        [HttpGet("reset-password/{id}")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            Response response = new(false);
            if (string.IsNullOrEmpty(id))
            {
                response.Message = Constant.INVALID_RESET_LINK;
                return BadRequest(response);
            }
            response = await _service.CheckResetLinkAsync(id);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO user)
        {
            User? userFound = await _service.FindUserByResetCodeAsync(user.ResetCode);
            Response response = new(true);
            if (userFound != null)
            {
                await _userService.UpdatePasswordAsync(userFound, user.NewPassword);
                response.Message = "Reset Password Successful";
                return Ok(response);
            }
            response.IsSuccess = false;
            response.Message = Constant.INVALID_RESET_LINK;
            return NotFound(response);
        }
    }
}