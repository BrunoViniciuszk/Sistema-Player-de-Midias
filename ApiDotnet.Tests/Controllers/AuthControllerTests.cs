using Xunit;
using Moq;
using api_dotnet.Controllers;
using api_dotnet.Models;
using api_dotnet.Models.Dtos;
using api_dotnet.Services.Auth;
using api_dotnet.Services.User;
using Microsoft.AspNetCore.Mvc;

public class AuthControllerTests
{
    private readonly Mock<IAppUserService> _userServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _userServiceMock = new Mock<IAppUserService>();
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_userServiceMock.Object, _authServiceMock.Object);
    }

    [Fact]
    public void Login_WhenValidUser_ReturnsToken()
    {
        var user = new AppUser { Username = "test" };
        _userServiceMock.Setup(s => s.Authenticate("user", "pass")).Returns(user);
        _authServiceMock.Setup(s => s.GenerateJwtToken(user)).Returns("token123");

        var result = _controller.Login(new LoginDto { Username = "user", Password = "pass" }) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Contains("token123", result.Value.ToString());
    }

    [Fact]
    public void Login_WhenInvalidUser_ReturnsUnauthorized()
    {
        _userServiceMock.Setup(s => s.Authenticate("user", "wrong")).Returns((AppUser)null);

        var result = _controller.Login(new LoginDto { Username = "user", Password = "wrong" });

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public void Register_WhenUserExists_ReturnsBadRequest()
    {
        _userServiceMock.Setup(s => s.GetByUsername("user")).Returns(new AppUser { Username = "user" });

        var result = _controller.Register(new RegisterDto { Username = "user", Password = "123" });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Register_WhenNewUser_ReturnsOk()
    {
        _userServiceMock.Setup(s => s.GetByUsername("newuser")).Returns((AppUser)null);

        var result = _controller.Register(new RegisterDto { Username = "newuser", Password = "123" }) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }
}
