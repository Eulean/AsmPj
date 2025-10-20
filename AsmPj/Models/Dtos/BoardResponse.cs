namespace AsmPj.Models.Dtos;

public class BoardResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }    
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }

    public List<TaskResponse>? Tasks { get; set; } = new List<TaskResponse>();
}