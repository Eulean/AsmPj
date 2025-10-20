namespace AsmPj.Models.Dtos;

public class TaskResponse
{
    public int Id { get; set; }
    public int BoardId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "To Do";
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set;}
    
}