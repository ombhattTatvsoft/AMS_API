
using Entity.DTOs;
using Entity.Models;

namespace BLL.IService
{
    public interface IUserService
    {
        Task<User?> FindUserByEmailAsync(string email);
        Task UpdatePasswordAsync(User user, string newPassword);
        Task<List<User>> GetAllUsersAsync();
        Task<UpsertUserDTO> GetUserAsync(int id);
        Task<Response> SaveUserAsync(UpsertUserDTO model);
        Task<List<User>> GetManagersByDepartmentAndRole(int departmentId, int roleId);
        Task<Response> DeleteUserAsync(int id);
        Task<User?> GetCurrentUserAsync();
        Task<List<User>> GetMyTeam(User user);
        Task<User?> GetUserById(int userId);
    }
}