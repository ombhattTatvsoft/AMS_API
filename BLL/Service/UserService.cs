using System.Net;
using System.Security.Claims;
using AutoMapper;
using BLL.Constants;
using BLL.Helpers;
using BLL.IService;
using DAL.IRepository;
using Entity.DTOs;
using Entity.Enums;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BLL.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ILogger<UserService> _logger;

    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Response> GetAllUsersAsync()
    {
        try
        {
            List<User> users = await _userRepository.GetAllAsync(x => !x.IsDeleted, x => x.Role, x => x.Manager, x => x.Department);
            return Response.Success(data: _mapper.Map<List<UserDTO>>(users));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            return Response.Failed();
        }
    }

    public Task UpdatePasswordAsync(User user, string newPassword)
    {
        try
        {
            var upsertedby = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier);
            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword, 13);
            user.ResetPasswordCode = null;
            user.ResetPasswordCodeExpiryTime = null;
            user.FirstLogin = false;
            user.Password = passwordHash;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = upsertedby == null ? user.UserId : int.Parse(upsertedby.Value);
            _userRepository.Update(user);
            return _userRepository.SaveAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating password for user {UserId}", user.UserId);
            return null;
        }
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        try
        {
            return await _userRepository.GetAsync(u => u.Email.ToLower() == email.ToLower() && !u.IsDeleted, x => x.Role);
        }
        catch (Exception e)
        {
            _logger.LogError(e, Constant.USER_BY_EMAIL_FAIL, email);
            return null;
        }
    }

    public async Task<Response> GetAllRolesAsync()
    {
        try
        {
            List<Role> roles = await _roleRepository.GetAllAsync();
            return Response.Success(data: _mapper.Map<List<RoleDTO>>(roles));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all roles");
            return Response.Failed();
        }
    }

    public async Task<Response> GetUserAsync(int id)
    {
        try
        {
            User? user = await _userRepository.GetAsync(x => !x.IsDeleted, x => x.Role, x => x.Manager, x => x.Department);
            return user == null ? Response.Failed(statusCode: HttpStatusCode.NotFound) : Response.Success(data: _mapper.Map<UserDTO>(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
            return Response.Failed();
        }
    }

    public async Task<Response> SaveUserAsync(UserDTO model)
    {
        try
        {
            int upsertedBy = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            User? userWithEmail = await FindUserByEmailAsync(model.Email);
            User? user = (model.UserId == 0) ? new User() : await _userRepository.GetAsync(u => u.UserId == model.UserId && !u.IsDeleted);
            if (userWithEmail != null && (model.UserId == 0 || (model.UserId != 0 && userWithEmail.UserId != model.UserId)))
                return Response.Failed("Email already exists", HttpStatusCode.BadRequest);
            if (model.UserId != 0 && user == null)
                return Response.Failed("User not found", HttpStatusCode.NotFound);
            user = model.UserId == 0 ? _mapper.Map<User>(model) : _mapper.Map(model, user);
            user!.UpdatedBy = upsertedBy;
            if (model.UserId == 0)
            {
                user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(Guid.NewGuid().ToString(), 13);
                user.CreatedBy = upsertedBy;
                await _userRepository.AddAsync(user);
            }
            else
            {
                user.UpdatedAt = DateTime.Now;
                _userRepository.Update(user);
            }
            await _userRepository.SaveAsync();
            if (model.DepartmentId == 0)
                return Response.Success("User created successfully", HttpStatusCode.Created, _mapper.Map<UserDTO>(user));
            return Response.Success("User updated successfully.", HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving user with email {Email}", model.Email);
            return Response.Failed();
        }
    }

    // public async Task<List<User>> GetManagersByDepartmentAndRole(int departmentId, int roleId)
    // {
    //     try
    //     {
    //         return await _userRepository.GetAllAsync(u => u.DepartmentId == departmentId && !u.IsDeleted && u.RoleId < roleId, u => u.Role);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error retrieving managers for department {DepartmentId}", departmentId);
    //         return new List<User>();
    //     }
    // }

    // public async Task<Response> DeleteUserAsync(int id)
    // {
    //     try
    //     {
    //         User? user = await _userRepository.GetAsync(x => x.UserId == id && !x.IsDeleted);
    //         if (user == null)
    //         {
    //             return new Response(false, "User not found");
    //         }
    //         if (await _userRepository.GetAsync(x => x.ManagerId == user.UserId && !x.IsDeleted) != null)
    //         {
    //             return new Response(false, "Cannot delete user who is a manager of other users");
    //         }
    //         user.IsDeleted = true;
    //         _userRepository.Update(user);
    //         await _userRepository.SaveAsync();
    //         return new Response(true, "User deleted successfully");
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
    //         return new Response(false, "An error occurred while deleting the user");
    //     }
    // }

    // public async Task<User?> GetCurrentUserAsync()
    // {
    //     try
    //     {
    //         return await _userRepository.GetAsync(x => x.UserId == int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!), x => x.InverseManager, x => x.Role);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "An error occurred while fetching the current user");
    //         return null;
    //     }
    // }

    // public async Task<List<User>> GetMyTeam(User user)
    // {
    //     try
    //     {
    //         if (user.Role.RoleName == EnumHelper.RoleToDbString(RoleEnum.Admin))
    //         {
    //             return await _userRepository.GetAllAsync(x => x.UserId != user.UserId && !x.IsDeleted, x => x.Role);
    //         }
    //         else
    //         {
    //             return await _userRepository.GetAllAsync(x => x.UserId != user.UserId && x.ManagerId == user.UserId && !x.IsDeleted, x => x.Role);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error retrieving team members for user {UserId}", user.UserId);
    //         return new List<User>();
    //     }
    // }

    // public async Task<User?> GetUserById(int userId)
    // {
    //     try
    //     {
    //         return await _userRepository.GetAsync(x => x.UserId == userId && !x.IsDeleted, x => x.Role);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error retrieving user {UserId}", userId);
    //         return new User();
    //     }
    // }

}
