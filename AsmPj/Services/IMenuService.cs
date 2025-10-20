using AsmPj.Helpers;
using AsmPj.Models.Dtos;

namespace AsmPj.Services;

public interface IMenuService
{
    Task<Result<IEnumerable<MenuResponse>>> GetMenuForUserAsync(int userId);
}
