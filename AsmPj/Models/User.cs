using Microsoft.EntityFrameworkCore;

namespace AsmPj.Models;

[Index(nameof(Name), IsUnique = false)]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(IsActive))]
[Index(nameof(IsDeleted))]
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    // public ICollection<Role>? Roles { get; set; } = new List<Role>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    

}