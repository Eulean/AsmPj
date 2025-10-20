namespace AsmPj.Services;

public interface IActivityLogService
{
    Task LogAsync(string action, string? performedBy, string? entityType = null, int? entityId = null );
}