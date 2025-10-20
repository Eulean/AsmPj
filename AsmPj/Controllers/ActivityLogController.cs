using AsmPj.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsmPj.Controllers;

[Authorize(Roles="Admin")]
[ApiController]
[Route("api/[controller]")]
public class ActivityLogController : ControllerBase
{
    private readonly MyDbContext _db;
    public ActivityLogController(MyDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetLogs(int page=1, int pageSize=50)
    {
        var logs = await _db.ActivityLogs
            .OrderByDescending(x => x.Timestamp)
            .Skip((page-1)*pageSize)
            .Take(pageSize)
            .ToListAsync();
        return Ok(new { success = true, data = logs });
    }
}
