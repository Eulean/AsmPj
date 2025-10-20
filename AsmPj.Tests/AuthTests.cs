using AsmPj.Data;
using AsmPj.Helpers;
using AsmPj.Models;
using AsmPj.Models.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsmPj.Tests;

public class AuthTests
{
    private readonly IMapper _mapper;

    public AuthTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    private MyDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new MyDbContext(options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }

    [Fact]
    public async Task RegisterUser_ShouldCreateNewUser()
    {
        //Arrange
        await using var context = GetDbContext();
        
        var role = new Role
        {
            Name = "User" ,
            Description = "Regular User",
            Created = DateTime.UtcNow
        };
        
        //Act
        context.Roles.Add(role);
        await context.SaveChangesAsync();
        
        var registerRequest = new RegisterRequest
        {
            Name = "Test User",
            Email = "testEmail@gmail.com",
            Password = "Test@1234",
            Role = "User"
        };
        
        // var existingRole = context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
        // Assert.NotNull(existingRole);
        
        //Manual Mapping
        var user = new User
        {
            Name = registerRequest.Name,
            Email = registerRequest.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
            Role = registerRequest.Role,
            DateCreated = DateTime.UtcNow,
        };
        
        //Act
        context.Users.Add(user);
        await context.SaveChangesAsync();
        
        //Assert
        var saved = await context.Users.FirstOrDefaultAsync(u => u.Email == registerRequest.Email);
        Assert.NotNull(saved);
        Assert.Equal(registerRequest.Name, saved.Name);
        Assert.Equal(registerRequest.Email, saved.Email);
        Assert.Equal(registerRequest.Role, saved.Role);
        Assert.True(BCrypt.Net.BCrypt.Verify(registerRequest.Password, saved.Password));


    }

    [Fact]
    public async Task LoginUser_ShouldReturnUser_WhenCredentialsAreValid()
    {
        //Arrange
        await using var context = GetDbContext();

        var loginRequest = new LoginRequest
        {
            Email = "testEmail@gmail.com",
            Password = "Test@1234",
        };
        
        var user = new User
        {
            Name = "Test User",
            Email = loginRequest.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(loginRequest.Password),
            Role = "User",
            DateCreated = DateTime.UtcNow,
        };
        
        var saved = await context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
        if (saved == null)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
        //Act
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
        //Assert
        Assert.NotNull(existingUser);
        Assert.Equal(user.Name, existingUser.Name);
        Assert.Equal(user.Email, existingUser.Email);
        Assert.Equal(user.Role, existingUser.Role);
        Assert.True(BCrypt.Net.BCrypt.Verify(loginRequest.Password, existingUser.Password));
        
    }
}