using AsmPj.Helpers;
using FluentAssertions;

namespace AsmPj.Tests.Helpers;

public class PasswordHasherTests
{
    [Fact]
    public void Hash_Should_Return_Hashed_Password()
    {
        // Arrange 
        var password = "MyPassword@123";
        
        // Act
        var hashed = PasswordHasher.Hash(password);
        
        // Assert
        hashed.Should().NotBeNullOrEmpty();
        hashed.Should().NotBe(password);
    }

    [Fact]
    public void Verify_Should_Return_Hashed_Password()
    {
        // Arrange 
        var password = "MyPassword@123";
        var hashed = PasswordHasher.Hash(password);
        
        // Act
        var verified = PasswordHasher.Verify(password, hashed);
        
        // Assert
        hashed.Should().NotBeNullOrEmpty();
        hashed.Should().NotBe(password);
        verified.Should().BeTrue();
        
        
    }

    [Fact]
    public void Verify_Should_Return_False_For_Wrong_Password()
    {
        // Arrange 
        var password = "MyPassword@123";
        var hashed = PasswordHasher.Hash(password);
        
        // Act 
        var result = PasswordHasher.Verify("WrongPassword",  hashed);
        
        // Assert 
        result.Should().BeFalse();
    }
}