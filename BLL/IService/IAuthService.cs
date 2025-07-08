using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Http;

namespace BLL.IService
{
    public interface IAuthService
    {
        Task<User?> AuthenticateUserAsync(string email, string password);
        string? CreateJwtToken(User user,bool rememberMe);
        string? PrepareLink(User userFound, HttpContext httpContext, bool? firstLogin = false);
        Response SendEmail(string email, string body, string subject);
        Task<Response> CheckResetLinkAsync(string id);
        Task<User?> FindUserByResetCodeAsync(string resetCode);
    }
}