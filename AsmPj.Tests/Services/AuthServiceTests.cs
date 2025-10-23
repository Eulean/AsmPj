using AsmPj.Data;
using AsmPj.Helpers;
using AsmPj.Models;
using AsmPj.Models.Dtos;
using AsmPj.Services;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace AsmPj.Tests;

public class AuthServiceTests
{
    private readonly IMapper _mapper;

    public AuthServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    private MyDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new MyDbContext(options);
        dbContext.Database.EnsureCreated();
        return dbContext;
        
    }

    [Fact]
    public async Task Register_ShouldCreateNewUser()
    {
        //Arrange
        var context = GetDbContext();
        var jwtServiceMock = new Mock<IJwtService>();
        var loggerMock = new Mock<ILogger<AuthService>>();
        var authService = new AuthService(context, jwtServiceMock.Object, loggerMock.Object,_mapper);

        var request = new RegisterRequest
        {
            Name = "John Doe",
            Email = "jhon@test.com",
            Password = "Test@123",
            Role = "User"
        };
        
        //Act
        var result = await authService.RegisterAsync(request);
        
        // Assert
        result.Should().NotBeNull();
        var saved = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        saved.Should().NotBeNull();
        saved?.Name.Should().Be("John Doe");
        BCrypt.Net.BCrypt.Verify("Test@123", saved.Password).Should().BeTrue();
    }
    
    [Fact]
    public async Task Login_ShouldReturnToken_WhenCredentialsValid()
    {
        // Arrange
        var context = GetDbContext();
        var jwtServiceMock = new Mock<IJwtService>();
        var loggerMock = new Mock<ILogger<AuthService>>();
        
        jwtServiceMock
            .Setup(j => j.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns("fake-jwt-token");

        var authService = new AuthService(context, jwtServiceMock.Object, loggerMock.Object, _mapper);

        var user = new User()
        {
            Name = "Jane",
            Email = "jane@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("Pass@123"),
            Role = "User",
            DateCreated = DateTime.UtcNow
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var request = new LoginRequest { Email = "jane@test.com", Password = "Pass@123" };

        // Act
        var response = await authService.LoginAsync(request);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Value.Token.Should().Be("fake-jwt-token");
        response.Value.Email.Should().Be("jane@test.com");
        response.Value.Role.Should().Be("User");
        
        // Verify mock usage
        jwtServiceMock.Verify(j => j.GenerateToken(
            It.IsAny<string>(),
            "jane@test.com",
            "User"
        ), Times.Once);
    }
}