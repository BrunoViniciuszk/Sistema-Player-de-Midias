using api_dotnet.Data;
using api_dotnet.Models;
using api_dotnet.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Linq;

public class AppUserServiceTests
{
    private readonly AppUserService _service;
    private readonly AppDbContext _context;

    public AppUserServiceTests()
    {
        // Cria banco de dados em memória isolado
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"UserServiceTestsDb_{System.Guid.NewGuid()}")
            .Options;

        var mockHasher = new Mock<IPasswordHasher<AppUser>>();
        mockHasher.Setup(h => h.HashPassword(It.IsAny<AppUser>(), It.IsAny<string>()))
                  .Returns((AppUser u, string pw) => $"hashed_{pw}");
        mockHasher.Setup(h => h.VerifyHashedPassword(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))
                  .Returns((AppUser u, string hashed, string provided) =>
                      hashed == $"hashed_{provided}" ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed);

        _context = new AppDbContext(options);
        _service = new AppUserService(_context, mockHasher.Object);
    }

    [Fact]
    public void Create_AddsUserSuccessfully()
    {
        var user = new AppUser { Username = "newuser" };
        _service.Create(user, "password123");

        var created = _context.Users.FirstOrDefault(u => u.Username == "newuser");
        Assert.NotNull(created);
        Assert.Equal("hashed_password123", created.PasswordHash);
    }

    [Fact]
    public void GetByUsername_WhenUserExists_ReturnsUser()
    {
        var user = new AppUser { Username = "existing", PasswordHash = "hashed_test" };
        _context.Users.Add(user);
        _context.SaveChanges();

        var result = _service.GetByUsername("existing");

        Assert.NotNull(result);
        Assert.Equal("existing", result.Username);
    }

    [Fact]
    public void Authenticate_WhenPasswordIsCorrect_ReturnsUser()
    {
        var user = new AppUser { Username = "authuser" };
        _service.Create(user, "mypassword");

        var result = _service.Authenticate("authuser", "mypassword");

        Assert.NotNull(result);
        Assert.Equal("authuser", result.Username);
    }

    [Fact]
    public void Authenticate_WhenPasswordIsWrong_ReturnsNull()
    {
        var user = new AppUser { Username = "authuser2" };
        _service.Create(user, "correctpass");

        var result = _service.Authenticate("authuser2", "wrongpass");

        Assert.Null(result);
    }
}
