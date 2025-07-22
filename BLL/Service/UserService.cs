using System.Net;
using System.Security.Claims;
using AutoMapper;
using static BLL.Constants.Constant;
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
            _logger.LogError(ex, GET_ALL_USERS_CATCH);
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
            _logger.LogError(ex, UPDATE_PASSWORD_CATCH, user.UserId);
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
            _logger.LogError(e, USER_BY_EMAIL_CATCH, email);
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
            _logger.LogError(ex, GET_ALL_ROLES_CATCH);
            return Response.Failed();
        }
    }

    public async Task<Response> GetUserAsync(int id)
    {
        try
        {
            User? user = await _userRepository.GetAsync(x => !x.IsDeleted && x.UserId == id, x => x.Role, x => x.Manager, x => x.Department);
            return user == null ? Response.Failed(statusCode: HttpStatusCode.NotFound) : Response.Success(data: _mapper.Map<UserDTO>(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, GET_USER_CATCH, id);
            return Response.Failed();
        }
    }

    public async Task<Response> SaveUserAsync(UserDTO model)
    {
        try
        {
            int upsertedBy = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            User? userWithEmail = await FindUserByEmailAsync(model.Email);
            User? user = (model.UserId == 0) ? new User() : await _userRepository.GetAsync(u => u.UserId == model.UserId && !u.IsDeleted);
            if (userWithEmail != null && (model.UserId == 0 || (model.UserId != 0 && userWithEmail.UserId != model.UserId)))
                return Response.Failed(USER_EXISTS, HttpStatusCode.BadRequest);
            if (model.UserId != 0 && user == null)
                return Response.Failed(USER_NOT_FOUND, HttpStatusCode.NotFound);
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
            if (model.UserId == 0)
                return Response.Success(USER_ADDED, HttpStatusCode.Created, user);
            return Response.Success(USER_UPDATED, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, SAVE_USER_CATCH, model.Email);
            return Response.Failed();
        }
    }

    public async Task<Response> DeleteUserAsync(int id)
    {
        try
        {
            User? user = await _userRepository.GetAsync(x => x.UserId == id && !x.IsDeleted);
            if (user == null)
            {
                return Response.Failed(USER_NOT_FOUND, HttpStatusCode.NotFound);
            }
            if (await _userRepository.GetAsync(x => x.ManagerId == user.UserId && !x.IsDeleted) != null)
            {
                return Response.Failed(ASSOCIATED_MANAGER, HttpStatusCode.BadRequest);
            }
            user.IsDeleted = true;
            _userRepository.Update(user);
            await _userRepository.SaveAsync();
            return Response.Success(USER_DELETED, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DELETE_USER_CATCH, id);
            return Response.Failed();
        }
    }

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

}
