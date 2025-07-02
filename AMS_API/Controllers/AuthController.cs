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
            if (userFound == null)
            {
                return Unauthorized(new { message = "Login Failed, Check Credentials!" });
            }
            string? token = _service.CreateJwtToken(userFound);
            if (token == null)
            {
                return StatusCode(500, new { message = "Login Failed, Please Try Again!" });
            }
            return Ok(new { message = "Login Successful", token });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO user)
        {
            User? userFound = await _userService.FindUserByEmailAsync(user.Email.Trim());
            if (userFound != null)
            {
                string? link = _service.PrepareLink(userFound, HttpContext);
                string subject = EmailBodyHelper.ForgotPasswordSubject;
                string body = EmailBodyHelper.ForgotPasswordBody(link!);

                Response response = _service.SendEmail(userFound.Email, body, subject);
                if (response.IsSuccess)
                    return Ok(new { message = response.Message });
                else
                    return StatusCode(500, new { message = response.Message });
            }
            else
            {
                return NotFound(new { message = "User Not Found." });
            }
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { message = "Invalid reset code." });

            Response response = await _service.CheckResetLinkAsync(id);
            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.Message });
            }
            return Ok(new ResetPasswordDTO { ResetCode = id });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO user)
        {

            User? userFound = await _service.FindUserByResetCodeAsync(user.ResetCode);
            if (userFound != null)
            {
                await _userService.UpdatePasswordAsync(userFound, user.NewPassword);
                return Ok(new { message = "Reset Password Successful" });
            }
            return NotFound(new { message = "User not found or invalid reset code" });
        }
    }
}