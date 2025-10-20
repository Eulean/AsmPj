using AsmPj.Helpers;
using AsmPj.Models;
using AsmPj.Models.Dtos;

namespace AsmPj.Services;

public interface IUserService
{
    Task<Result<IEnumerable<UserResponse>>> GetAllUsersAsync(int page, int pageSize);
    Task<Result<UserResponse>> GetByIdAsync(int id);
    Task<Result<UserResponse>> GetByUsernameAsync(string username);
    Task<Result<UserResponse>> GetByEmailAsync(string email);
    Task<Result<string>> ActivateUserAsync(int id);
    Task<Result<string>> DeactivateUserAsync(int id);
    // Task<Result<string>> UpdateUserAsync(int id, UpdateUserRequest request);
    // Task<Result> DeleteUserAsync(int id);
}