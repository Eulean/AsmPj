using AsmPj.Models;
using Microsoft.EntityFrameworkCore;

namespace AsmPj.Data;

public static class SeedData
{
    public static void Initialize(ModelBuilder modelBuilder)
    {
        // Roles
        var adminRole = new Role
        {
            Id = 1,
            Name = "Admin",
            Description = "Full system access",
            Created = DateTime.UtcNow,
            IsActive = true,
        };

        var userRole = new Role
        {
            Id = 2,
            Name = "User",
            Description = "Standard user access",
            Created = DateTime.UtcNow,
            IsActive = true,
        };
        
  
        // Link Admin user to Admin role
        modelBuilder.Entity<UserRole>().HasData(
                    new UserRole { UserId = 1, RoleId = 1 }
                    );
        
        // Permissions
        var permissions = new List<Permission>
        {
            new Permission { Id = 1, Name = "ViewDashboard", Created = DateTime.UtcNow },
            new Permission { Id = 2, Name = "ManageUsers", Created = DateTime.UtcNow },
            new Permission { Id = 3, Name = "ManageRoles", Created = DateTime.UtcNow },
            new Permission { Id = 4, Name = "ViewReports", Created = DateTime.UtcNow },
        };
        
        // Menus
        var menus = new List<Menu>
        {
            new Menu{Id = 1, Name = "Dashboard", Created = DateTime.UtcNow},
            new Menu{Id = 2, Name = "Users", Created = DateTime.UtcNow},
            new Menu{Id = 3, Name = "Roles", Created = DateTime.UtcNow},
            new Menu{Id = 4, Name = "Reports", Created = DateTime.UtcNow},
        };
        
        // Seed the explicit join entity PermissionMenu (composite PK PermissionId + MenuId)
        modelBuilder.Entity<PermissionMenu>().HasData(
            new PermissionMenu { PermissionId = 1, MenuId = 1 },
            new PermissionMenu { PermissionId = 2, MenuId = 2 },
            new PermissionMenu { PermissionId = 3, MenuId = 3 },
            new PermissionMenu { PermissionId = 4, MenuId = 4 }
        );
        
        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = 1, PermissionId = 1 },
            new RolePermission { RoleId = 1, PermissionId = 2 },
            new RolePermission { RoleId = 1, PermissionId = 3 },
            new RolePermission { RoleId = 1, PermissionId = 4 },
            new RolePermission { RoleId = 2, PermissionId = 2 }
        );

        
        // Admin User
        var adminUser = new User
        {
            Id = 1,
            Name = "System Admin",
            Email = "nightruner115@gmail.com",
            Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = "Admin",
            DateCreated = DateTime.UtcNow,
            IsActive = true
        };
        
        // Add to Model
        modelBuilder.Entity<Role>().HasData(adminRole, userRole);
        modelBuilder.Entity<User>().HasData(adminUser);
        modelBuilder.Entity<Permission>().HasData(permissions);
        modelBuilder.Entity<Menu>().HasData(menus);
    }
}