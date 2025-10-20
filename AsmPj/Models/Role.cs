using Microsoft.EntityFrameworkCore;

namespace AsmPj.Models;

[Index(nameof(Name) , IsUnique = true)]
[Index(nameof(IsActive))]
public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    // public ICollection<User>? Users { get; set; } = new List<User>();
    public ICollection<RolePermission>? RolePermissions { get; set; } = new List<RolePermission>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}