using System.ComponentModel.DataAnnotations;

namespace AsmPj.Models;

public class Board
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    [MaxLength(500)]
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    
    [Required]
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}