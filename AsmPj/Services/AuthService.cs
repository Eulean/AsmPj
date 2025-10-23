using AsmPj.Data;
using AsmPj.Helpers;
using AsmPj.Models;
using AsmPj.Models.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsmPj.Services;

public class AuthService : IAuthService
{
    private readonly MyDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    
    public AuthService(MyDbContext context, IJwtService jwtService, ILogger<AuthService> logger, IMapper mapper)
    {
        _context = context;
        _jwtService = jwtService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<string>> RegisterAsync(RegisterRequest request)
    {
        try
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                _logger.LogWarning("Registration failed for {Email}: Email already exists", request.Email);
                return Result<string>.Failure("Email already exists", AppErrorCodes.EmailExists);
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == request.Role);
            if (role == null)
            {
                _logger.LogWarning("Registration failed for {Email}: Role not found", request.Email);
                return Result<string>.Failure("Role not found", AppErrorCodes.RoleNotFound);
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role,
                DateCreated = DateTime.UtcNow,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
            };

            _context.Set<UserRole>().Add(userRole);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User registered successfully: {Email}", request.Email);
            return Result<string>.Success("User registered successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for {Email}", request.Email);
            return Result<string>.Failure("An error occurred during registration", AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed for {Email}: User not found", request.Email);
                return Result<LoginResponse>.Failure("Invalid credentials", AppErrorCodes.InvalidCredentials);
            }

            var verified = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!verified)
            {
                _logger.LogWarning("Login failed for {Email}: Invalid password", request.Email);
                return Result<LoginResponse>.Failure("Invalid credentials", AppErrorCodes.InvalidCredentials);
            }

            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, user.Role);

            var response = new LoginResponse
            {
                Token = token,
                Email = user.Email,
                Role = user.Role
            };
            
            _logger.LogInformation("User logged in successfully: {Email}", request.Email);
            return Result<LoginResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for {Email}", request.Email);
            return Result<LoginResponse>.Failure("An error occurred during login", AppErrorCodes.ServerError);
        }
    }
}