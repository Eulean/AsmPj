using AsmPj.Helpers;
using AsmPj.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsmPj.Controllers;

/// <summary>
///     Handle User Mangement (Get All Users, Get My Profile, Activate/Deactivate User)
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    ///     Get all users (Admin only)
    /// </summary>
    /// <returns>Success or Error message</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers([FromQuery]int page = 1,[FromQuery] int pageSize = 10)
    {
        var result = await _userService.GetAllUsersAsync(page, pageSize);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Get My Profile
    /// </summary>
    /// <returns>Success</returns>
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var email = User?.Claims?.FirstOrDefault(c => c.Type == "email" || c.Type.Contains("email"))?.Value;
        if (email == null)
            return Unauthorized(new { error = "Invalid token." });

        var result = await _userService.GetByEmailAsync(email);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Deactivate a user (Admin only)
    /// </summary>
    /// <param name="id">Deactivate User (userid) </param>
    /// <returns>Success or ErrorMessage</returns>
    [Authorize(Roles = "Admin")]
    [HttpPut("deactivate/{id}")]
    public async Task<IActionResult> DeactivateUser(int id)
    {
        var result = await _userService.DeactivateUserAsync(id);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Activate a user (Admin only)
    /// </summary>
    /// <param name="id">Activate User (userid)</param>
    /// <returns>Success or ErrorMessage</returns>
    [Authorize(Roles = "Admin")]
    [HttpPut("activate/{id}")]
    public async Task<IActionResult> ActivateUser(int id)
    {
        var result = await _userService.ActivateUserAsync(id);
        return result.ToActionResult();
    }
}