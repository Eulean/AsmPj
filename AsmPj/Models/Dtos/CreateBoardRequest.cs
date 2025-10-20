using System.ComponentModel.DataAnnotations;

namespace AsmPj.Models.Dtos;

public class CreateBoardRequest
{
   public string Name { get; set; } = string.Empty;
   public string? Description { get; set; }
}