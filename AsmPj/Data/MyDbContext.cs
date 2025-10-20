using AsmPj.Models;
using Microsoft.EntityFrameworkCore;

namespace AsmPj.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<RolePermission> RolePermission{ get; set; }
    public DbSet<PermissionMenu> PermissionMenu { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure many-to-many relationships
        
        // modelBuilder.Entity<User>()
        //     .HasMany(u => u.Roles)
        //     .WithMany(r => r.Users)
        //     .UsingEntity(j => j.ToTable("UserRoles"));
        
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
        
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
        
        // modelBuilder.Entity<Role>()
        //     .HasMany(r => r.Permissions)
        //     .WithMany(p => p.Roles)
        //     .UsingEntity(j => j.ToTable("RolePermissions"));

        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });
        
        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);

        // modelBuilder.Entity<Permission>()
        //     .HasMany(p => p.Menus)
        //     .WithMany(m => m.Permissions)
        //     .UsingEntity(j => j.ToTable("PermissionMenus"));
        
        modelBuilder.Entity<PermissionMenu>()
            .HasKey(pm => new { pm.PermissionId, pm.MenuId });

        modelBuilder.Entity<PermissionMenu>()
            .HasOne(pm => pm.Permission)
            .WithMany(p => p.PermissionMenus)
            .HasForeignKey(pm => pm.PermissionId);

        modelBuilder.Entity<PermissionMenu>()
            .HasOne(pm => pm.Menu)
            .WithMany(m => m.PermissionMenus)
            .HasForeignKey(pm => pm.MenuId);

        
        // modelBuilder.Entity<Menu>()
        //     .HasMany(m => m.Permissions)
        //     .WithMany(p => p.Menus)
        //     .UsingEntity(j => j.ToTable("PermissionMenus"));
        
       // call Seeder
       SeedData.Initialize(modelBuilder);
    }
    
    
}
    
    
