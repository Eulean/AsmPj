using AsmPj.Helpers;
using AsmPj.Models.Dtos;
using AsmPj.Services;
using Microsoft.AspNetCore.Mvc;

namespace AsmPj.Controllers;
/// <summary>
/// Handles user authentication including registration and login.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="request">User registration data (Name , Email , Password ,Role)</param>
    /// <returns>Success or Error Message</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        #region Service Code

        var result = await _authService.RegisterAsync(request);
        return result.ToActionResult();

        #endregion

        #region Manual Code

        // if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        //     return BadRequest("Email already exists");
        //
        // var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == request.Role);
        // if (role == null) return BadRequest("Role not found");
        //
        //
        // var user = new User
        // {
        //     Name = request.Name,
        //     Email = request.Email,
        //     Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
        //     Role = request.Role,
        //     DateCreated = DateTime.UtcNow
        // };
        //
        // // user.UserRoles.Add(role);
        //
        // _context.Users.Add(user);
        // await _context.SaveChangesAsync();
        //
        // var userRole = new UserRole
        // {
        //     UserId = user.Id,
        //     RoleId = role.Id
        // };
        //
        // _context.Set<UserRole>().Add(userRole);
        // await _context.SaveChangesAsync();
        //
        // return Ok("User registered successfully");

        #endregion
    }

    /// <summary>
    /// Authenticate a user and generate a JWT token.
    /// </summary>
    /// <param name="request">Login credentials (Email, Password)</param>
    /// <returns>JWT token with user role and email</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        #region Service Code

        var result = await _authService.LoginAsync(request);
        return result.ToActionResult();

        #endregion

        #region Manul code


        // var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        // if (user == null) return Unauthorized("Invalid credentials.");
        //
        // var verified = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        // if (!verified) return Unauthorized("Invalid credentials.");
        //
        // var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, user.Role);
        //
        // return Ok(new LoginResponse
        // {
        //     Token = token,
        //     // Name = user.Name,
        //     Email = user.Email,
        //     Role = user.Role
        // });

        #endregion
    }
}