
using Entity.DTOs;
using Entity.Models;

namespace BLL.IService
{
    public interface IUserService
    {
        Task<Response> GetAllUsersAsync();
        Task<Response> GetAllRolesAsync();
        Task<Response> GetUserAsync(int id);
        Task<Response> SaveUserAsync(UserDTO model);
        Task<Response> DeleteUserAsync(int id);
        Task<User?> FindUserByEmailAsync(string email);
        Task UpdatePasswordAsync(User user, string newPassword);
        // Task<List<User>> GetMyTeam(User user);
    }
}