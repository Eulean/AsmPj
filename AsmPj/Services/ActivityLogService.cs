using AsmPj.Data;
using AsmPj.Models;

namespace AsmPj.Services;

public class ActivityLogService : IActivityLogService
{
    private readonly MyDbContext _context;
    
    public ActivityLogService(MyDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(string action, string? performedBy, string? entityType = null, int? entityId = null)
    {
        var log = new ActivityLog
        {
            Action = action,
            PerformedBy = performedBy,
            EntityType = entityType,
            EntityId = entityId,
            Timestamp = DateTime.UtcNow
        };

        _context.ActivityLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}