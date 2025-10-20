using AsmPj.Data;
using AsmPj.Helpers;
using AsmPj.Models;
using AsmPj.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AsmPj.Services;

public class MenuService : IMenuService
{
    private readonly MyDbContext _context;
    private readonly ILogger<MenuService> _logger;
    
    public MenuService(MyDbContext context, ILogger<MenuService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<MenuResponse>>> GetMenuForUserAsync(int userId)
    {
        try
        {
            var roleIds = await _context.UserRole
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            if (!roleIds.Any())
                return Result<IEnumerable<MenuResponse>>.Failure("User has no roles assigned",
                    AppErrorCodes.AssesDenied);

            var permissionIds = await _context.RolePermission
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Select(rp => rp.PermissionId)
                .Distinct()
                .ToListAsync();

            var menus = await _context.PermissionMenu
                .Where(pm => permissionIds.Contains(pm.PermissionId))
                .Include(pm => pm.Menu)
                .Select(pm => new MenuResponse
                {
                    Id = pm.MenuId,
                    Name = pm.Menu.Name,
                    Description = pm.Menu.Description
                })
                .Distinct()
                .ToListAsync();
            
            _logger.LogInformation("Fetched {Count} menus for user {UserId}", menus.Count, userId);
            return Result<IEnumerable<MenuResponse>>.Success(menus);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching menus for user {UserId}", userId);
            return Result<IEnumerable<MenuResponse>>.Failure("Error fetching menus.", AppErrorCodes.ServerError);
        }
    }
}