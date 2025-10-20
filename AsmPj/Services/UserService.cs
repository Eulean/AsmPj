using AsmPj.Data;
using AsmPj.Helpers;
using AsmPj.Models;
using AsmPj.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AsmPj.Services;

public class UserService : IUserService
{
    private readonly MyDbContext _context;
    private readonly ILogger<UserService> _logger;
    
    public UserService(MyDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<UserResponse>>> GetAllUsersAsync(int page, int pageSize)
    {
        try
        {
            var users = await _context.Users
                .AsNoTracking()
                .Skip((page -1 ) * pageSize)
                .Take(pageSize)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    IsDeleted = u.IsDeleted,
                    DateCreated = u.DateCreated
                })
                .ToListAsync();

            return Result<IEnumerable<UserResponse>>.Success(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all users");
            return Result<IEnumerable<UserResponse>>.Failure("Error fetching users",AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<UserResponse>> GetByIdAsync(int id)
    {
        try
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    IsDeleted = u.IsDeleted,
                    DateCreated = u.DateCreated
                })
                .FirstOrDefaultAsync();

            if (user != null) return Result<UserResponse>.Success(user);
            
            _logger.LogWarning("User with ID {UserId} not found", id);
            return Result<UserResponse>.Failure("User not found",AppErrorCodes.UserNotFound);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user by ID");
            return Result<UserResponse>.Failure("Error fetching user",AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<UserResponse>> GetByUsernameAsync(string username)
    {
        try
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Name == username)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    IsDeleted = u.IsDeleted,
                    DateCreated = u.DateCreated
                })
                .FirstOrDefaultAsync();
            
            if (user != null) return Result<UserResponse>.Success(user);
            
            _logger.LogWarning("User with UserName {UserName} not found", username);
            return Result<UserResponse>.Failure("User not found",AppErrorCodes.UserNotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user by UserName");
            return Result<UserResponse>.Failure("Error fetching user",AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<UserResponse>> GetByEmailAsync(string email)
    {
        try
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Email == email)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    IsDeleted = u.IsDeleted,
                    DateCreated = u.DateCreated
                })
                .FirstOrDefaultAsync();
            
            if (user != null) return Result<UserResponse>.Success(user);
            
            _logger.LogWarning("User with Email {email} not found", email);
            return Result<UserResponse>.Failure("User not found",AppErrorCodes.UserNotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user by Email");
            return Result<UserResponse>.Failure("Error fetching user",AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<string>> ActivateUserAsync(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) 
                return Result<string>.Failure("User not found",AppErrorCodes.UserNotFound);

            user.IsActive = true;
            user.DateModified = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            return Result<string>.Success("User activated");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Activating user with ID: {id}", id);
            return Result<string>.Failure("Error Activating user",AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<string>> DeactivateUserAsync(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) 
                return Result<string>.Failure("User not found",AppErrorCodes.UserNotFound);

            user.IsActive = false;
            user.DateModified = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            return Result<string>.Success("User deactivated");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Deactivating user with ID: {id}", id);
            return Result<string>.Failure("Error fetching user",AppErrorCodes.ServerError);
        }
    }
}