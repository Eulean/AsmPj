using System.ComponentModel.DataAnnotations;

namespace AsmPj.Models;

public class ActivityLog
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;
    [MaxLength(100)]
    public string? PerformedBy { get; set; }
    [MaxLength(50)]
    public string? EntityType { get; set; }
    public int? EntityId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}