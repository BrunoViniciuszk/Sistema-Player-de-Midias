using Xunit;
using api_dotnet.Services.Auth;
using api_dotnet.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

public class AuthServiceTests
{
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        
        var inMemorySettings = new Dictionary<string, string> {
            {"JwtSettings:SecretKey", "MinhaChaveSuperSecretaCom123!@#2025"}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _authService = new AuthService(configuration);
    }

    [Fact]
    public void GenerateJwtToken_WhenUserIsValid_ReturnsToken()
    {
        // Arrange
        var user = new AppUser { Id = 1, Username = "testuser" };

        // Act
        var token = _authService.GenerateJwtToken(user);

        // Assert
        Assert.False(string.IsNullOrEmpty(token));
        Assert.Equal(3, token.Split('.').Length); // verifica que é um JWT válido
    }
}
