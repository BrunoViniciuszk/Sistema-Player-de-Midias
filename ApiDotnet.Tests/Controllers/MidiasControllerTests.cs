using Xunit;
using Moq;
using api_dotnet.Controllers;
using api_dotnet.Services.Midias;
using api_dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

public class MidiasControllerTests
{
    private readonly Mock<IMidiaService> _midiaServiceMock;
    private readonly MidiasController _controller;

    public MidiasControllerTests()
    {
        _midiaServiceMock = new Mock<IMidiaService>();
        _controller = new MidiasController(_midiaServiceMock.Object);
    }

    [Fact]
    public async Task Get_ReturnsOkWithMidias()
    {
        _midiaServiceMock.Setup(s => s.GetAllAsync())
            .ReturnsAsync(new List<Midia> { new Midia { Id = 1, Nome = "Teste" } });

        var result = await _controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var midias = Assert.IsAssignableFrom<IEnumerable<Midia>>(okResult.Value);
        Assert.Single(midias);
    }

    [Fact]
    public async Task GetMidia_WhenExists_ReturnsOk()
    {
        _midiaServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new Midia { Id = 1, Nome = "Teste" });

        var result = await _controller.GetMidia(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var midia = Assert.IsType<Midia>(okResult.Value);
        Assert.Equal("Teste", midia.Nome);
    }

    [Fact]
    public async Task GetMidia_WhenNotFound_ReturnsNotFound()
    {
        _midiaServiceMock.Setup(s => s.GetByIdAsync(1)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.GetMidia(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Delete_WhenNotFound_ReturnsNotFound()
    {
        _midiaServiceMock.Setup(s => s.DeleteAsync(1)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.Delete(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_WhenExists_ReturnsNoContent()
    {
        _midiaServiceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_WhenNotFound_ReturnsNotFound()
    {
        _midiaServiceMock.Setup(s => s.UpdateAsync(1, "Novo", "Desc", null))
            .ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.Update(1, "Novo", "Desc", null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_WhenSuccessful_ReturnsOk()
    {
        var updatedMidia = new Midia { Id = 1, Nome = "Novo" };
        _midiaServiceMock.Setup(s => s.UpdateAsync(1, "Novo", "Desc", null))
            .ReturnsAsync(updatedMidia);

        var result = await _controller.Update(1, "Novo", "Desc", null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var midia = Assert.IsType<Midia>(okResult.Value);
        Assert.Equal("Novo", midia.Nome);
    }

    [Fact]
    public async Task Upload_WhenSuccessful_ReturnsCreated()
    {
        var fileMock = new Mock<IFormFile>();
        var midia = new Midia { Id = 1, Nome = "Nova" };

        _midiaServiceMock.Setup(s => s.UploadAndCreateMidiaAsync(fileMock.Object, "Nova", "Desc"))
            .ReturnsAsync(midia);

        var result = await _controller.Upload(fileMock.Object, "Nova", "Desc");

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var resultMidia = Assert.IsType<Midia>(createdResult.Value);
        Assert.Equal("Nova", resultMidia.Nome);
    }

    [Fact]
    public async Task Upload_WhenArgumentException_ReturnsBadRequest()
    {
        var fileMock = new Mock<IFormFile>();

        _midiaServiceMock.Setup(s => s.UploadAndCreateMidiaAsync(fileMock.Object, "Nova", "Desc"))
            .ThrowsAsync(new ArgumentException("Erro"));

        var result = await _controller.Upload(fileMock.Object, "Nova", "Desc");

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro", badRequest.Value);
    }
}
