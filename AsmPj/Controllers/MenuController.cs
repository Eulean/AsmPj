using System.Security.Claims;
using AsmPj.Helpers;
using AsmPj.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsmPj.Controllers;
[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class MenuController : Controller
{
    private readonly IMenuService _menuService;
    
    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet("my-menus")]
    public async Task<IActionResult> GetMyMenus()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized(AppErrorCodes.InvalidToken);

        var result = await _menuService.GetMenuForUserAsync(int.Parse(userIdClaim));

        return result.ToActionResult();
    }
}