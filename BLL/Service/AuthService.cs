using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using static BLL.Constants.Constant;
using BLL.IService;
using DAL.IRepository;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Service;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    private readonly IConfiguration _config;

    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, IConfiguration config, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _config = config;
        _logger = logger;
    }

    public async Task<User?> AuthenticateUserAsync(string email, string password)
    {
        try
        {
            User? user = await _userRepository.GetAsync(x => x.Email.ToLower() == email.ToLower() && !x.IsDeleted, x => x.Role);
            if (user != null && BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password))
            {
                return user;
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AUTH_USER_CATCH, email);
            return null;
        }
    }

    public string? CreateJwtToken(User user, bool rememberMe)
    {
        try
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Role, user.Role.RoleName)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(rememberMe ? 30 : 1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, TOKEN_CREATE_CATCH, user.UserId);
            return null;
        }
    }

    public string? PrepareLink(User userFound, HttpContext httpContext, bool? firstLogin = false)
    {
        try
        {
            string resetCode = Guid.NewGuid().ToString();
            string verifyUrl = RESET_PASS_PATH + resetCode;
            string link = $"http://localhost:5173/{verifyUrl}";
            userFound.ResetPasswordCode = resetCode;
            if (firstLogin == true)
                userFound.FirstLogin = true;
            else
                userFound.ResetPasswordCodeExpiryTime = DateTime.Now.AddHours(24);
            _userRepository.Update(userFound);
            _userRepository.SaveAsync();
            return link;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, EMAIL_LINK_CATCH);
            return null;
        }
    }

    public Response SendEmail(string email, string body, string subject)
    {
        try
        {
            using MailMessage mm = new(_config["MailMessageCred:Email"]!, email);
            mm.Subject = subject;
            mm.Body = body;
            mm.IsBodyHtml = true;
            SmtpClient smtp = new()
            {
                Host = _config["MailMessageCred:Host"]!,
                EnableSsl = true
            };
            NetworkCredential NetworkCred = new(_config["MailMessageCred:Email"]!, _config["MailMessageCred:Password"]!);
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mm);
            return Response.Success(EMAIL_SUCCESS, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, EMAIL_SEND_CATCH, subject);
            return Response.Failed(EMAIL_FAIL);
        }
    }

    public async Task<Response> CheckResetLinkAsync(string id)
    {
        try
        {
            User? user = await _userRepository.GetAsync(x => x.ResetPasswordCode == id && !x.IsDeleted);
            if (user == null)
                return new Response(false, INVALID_RESET_LINK);
            else
            {
                if (!user.FirstLogin && (user.ResetPasswordCodeExpiryTime == null || DateTime.Now > user.ResetPasswordCodeExpiryTime))
                {
                    user.ResetPasswordCode = null;
                    user.ResetPasswordCodeExpiryTime = null;
                    _userRepository.Update(user);
                    return new Response(false, EXPIRED_RESET_LINK);
                }
                return new Response(true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, CHECK_RESET_LINK_CATCH, id);
            return new Response(false, CHECK_RESET_LINK_CATCH,id);
        }
    }

    public async Task<User?> FindUserByResetCodeAsync(string resetCode)
    {
        try
        {
            return await _userRepository.GetAsync(x => x.ResetPasswordCode == resetCode && !x.IsDeleted);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, USER_BY_RESET_CODE_CATCH, resetCode);
            return null;
        }
    }
}
