using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
            _logger.LogError(ex, "Error authenticating user with email: {Email}", email);
            return null;
        }
    }

    public string? CreateJwtToken(User user,bool rememberMe)
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
            _logger.LogError(e, "Error creating JWT token for user: {UserId}", user.UserId);
            return null;
        }
    }

    public string? PrepareLink(User userFound, HttpContext httpContext, bool? firstLogin = false)
    {
        try
        {
            string resetCode = Guid.NewGuid().ToString();
            string verifyUrl = "/reset-password/" + resetCode;
            string link = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{verifyUrl}";
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
            _logger.LogError(ex, "Email reset link preparation failed");
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
            return new Response(true, "Email sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email sent failed for {Subject}", subject);
            return new Response(false, "Email sent failed");
        }
    }

    public async Task<Response> CheckResetLinkAsync(string id)
    {
        try
        {
            User? user = await _userRepository.GetAsync(x => x.ResetPasswordCode == id && !x.IsDeleted);
            if (user == null)
                return new Response(false, "Invalid Link");
            else
            {
                if (!user.FirstLogin && (user.ResetPasswordCodeExpiryTime == null || DateTime.Now > user.ResetPasswordCodeExpiryTime))
                {
                    user.ResetPasswordCode = null;
                    user.ResetPasswordCodeExpiryTime = null;
                    _userRepository.Update(user);
                    return new Response(false, "Reset Link Expired.");
                }
                return new Response(true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking reset link for ID: {Id}", id);
            return new Response(false, "An error occurred while checking the reset link.");
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
            _logger.LogError(ex, "Error finding user by reset code: {ResetCode}", resetCode);
            return null;
        }
    }
}
