using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AsmPj.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace AsmPj.Tests.Utils;

public class JwtServiceTests
{
    private JwtService GetJwtService()
    {
        var settings = new Dictionary<string, string?>
        {
            { "Jwt:Key", "super_secret_jwt_key_please_change" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings!)
            .Build();

        return new JwtService(configuration);
    }
    
    [Fact]
    public void GenerateToken_Should_Return_Valid_JWT_With_Claims()
    {
        // Arrange
        var jwtService = GetJwtService();

        // Act
        var tokenString = jwtService.GenerateToken("123", "test@example.com", "Admin");

        // Assert
        tokenString.Should().NotBeNullOrEmpty();

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);

        token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value.Should().Be("123");
        token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value.Should().Be("test@example.com");
        token.Claims.First(c => c.Type == ClaimTypes.Role).Value.Should().Be("Admin");
        token.Issuer.Should().Be("TestIssuer");
    }

    [Fact]
    public void GenerateToken_Should_Have_Valid_Expiration()
    {
        // Arrange
        var jwtService = GetJwtService();

        // Act
        var tokenString = jwtService.GenerateToken("123", "test@example.com", "User");

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);

        token.ValidTo.Should().BeAfter(DateTime.UtcNow);
        (token.ValidTo - DateTime.UtcNow).TotalMinutes.Should().BeApproximately(180, precision: 5);
    }
}
