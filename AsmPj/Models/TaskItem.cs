using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsmPj.Models;

public class TaskItem
{
    public int Id { get; set; }
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required]
    public string Status { get; set; } = "To Do";
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }

    [Required] 
    public int BoardId { get; set; }
    [ForeignKey("BoardId")]
    public Board? Board { get; set; } = null!;
}