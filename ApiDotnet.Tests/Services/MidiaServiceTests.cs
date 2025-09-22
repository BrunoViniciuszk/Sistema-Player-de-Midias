using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using api_dotnet.Models;
using api_dotnet.Services.Midias;
using api_dotnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

public class MidiaServiceTests
{
    private readonly Mock<IMidiaRepository> _midiaRepoMock;
    private readonly Mock<IWebHostEnvironment> _envMock;
    private readonly MidiaService _service;

    public MidiaServiceTests()
    {
        _midiaRepoMock = new Mock<IMidiaRepository>();
        _envMock = new Mock<IWebHostEnvironment>();
        _envMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

        _service = new MidiaService(_midiaRepoMock.Object, _envMock.Object);
    }

    
    private IFormFile CreateFakeFile(string fileName, string content = "dummy content")
    {
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
        return new FormFile(stream, 0, stream.Length, "file", fileName);
    }

    
    [Fact]
    public async Task GetAllAsync_ReturnsMidias()
    {
        
        var midias = new List<Midia> { new Midia { Id = 1, Nome = "Teste" } };
        _midiaRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(midias);

        
        var result = await _service.GetAllAsync();

        
        Assert.Single(result);
        Assert.Equal("Teste", ((List<Midia>)result)[0].Nome);
    }

    
    [Fact]
    public async Task GetByIdAsync_WhenExists_ReturnsMidia()
    {
        var midia = new Midia { Id = 1, Nome = "Música A" };
        _midiaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(midia);

        var result = await _service.GetByIdAsync(1);

        Assert.Equal("Música A", result.Nome);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ThrowsKeyNotFoundException()
    {
        _midiaRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                      .ReturnsAsync((Midia)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(1));
    }

    
    [Fact]
    public async Task UpdateAsync_WhenExists_UpdatesFields()
    {
        
        var midia = new Midia { Id = 1, Nome = "Antigo", Descricao = "Velha" };
        _midiaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(midia);

        
        var result = await _service.UpdateAsync(1, "Novo", "Nova desc", null);

        
        Assert.Equal("Novo", result.Nome);
        Assert.Equal("Nova desc", result.Descricao);
        _midiaRepoMock.Verify(r => r.UpdateAsync(midia), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ThrowsKeyNotFoundException()
    {
        _midiaRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                      .ReturnsAsync((Midia)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(1, "Novo", null, null));
    }

    [Fact]
    public async Task UpdateAsync_WhenFileProvided_UpdatesUrlMidia()
    {
        
        var midia = new Midia { Id = 1, Nome = "Teste", Descricao = "Desc" };
        _midiaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(midia);
        var file = CreateFakeFile("teste.jpg");

        
        var result = await _service.UpdateAsync(1, null, null, file);

        
        Assert.Contains("/Uploads/Imagens/", result.UrlMidia);
        _midiaRepoMock.Verify(r => r.UpdateAsync(midia), Times.Once);
    }

    
    [Fact]
    public async Task DeleteAsync_WhenExists_DeletesMidia()
    {
        var midia = new Midia { Id = 1, Nome = "Teste", UrlMidia = "Uploads/Imagens/teste.jpg" };
        _midiaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(midia);

        await _service.DeleteAsync(1);

        _midiaRepoMock.Verify(r => r.DeleteAsync(midia), Times.Once);
        _midiaRepoMock.Verify(r => r.RemoveFromPlaylistsAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ThrowsKeyNotFoundException()
    {
        _midiaRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                      .ReturnsAsync((Midia)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(1));
    }

    [Fact]
    public async Task UploadAndCreateMidiaAsync_WhenFileIsValidImage_CreatesMidia()
    {
        var file = CreateFakeFile("foto.png");

        var resultMidia = new Midia { Id = 1, Nome = "Nome", Descricao = "Desc", UrlMidia = "/Uploads/Imagens/teste.png" };
        _midiaRepoMock.Setup(r => r.CreateAsync(It.IsAny<Midia>()))
                      .ReturnsAsync(resultMidia);

        var result = await _service.UploadAndCreateMidiaAsync(file, "Nome", "Desc");

        Assert.Equal("Nome", result.Nome);
        Assert.Contains("/Uploads/Imagens/", result.UrlMidia);
    }

    [Fact]
    public async Task UploadAndCreateMidiaAsync_WhenFileIsNull_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _service.UploadAndCreateMidiaAsync(null, "Nome", "Desc"));
    }

    [Fact]
    public async Task UploadAndCreateMidiaAsync_WhenFileHasUnsupportedExtension_ThrowsArgumentException()
    {
        var file = CreateFakeFile("arquivo.txt");

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UploadAndCreateMidiaAsync(file, "Nome", "Desc"));
    }

    [Fact]
    public async Task UpdateAsync_WhenNomeAndDescricaoAreNull_KeepsOriginalValues()
    {
        
        var midia = new Midia { Id = 1, Nome = "Original", Descricao = "Descricao original" };
        _midiaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(midia);

        
        var result = await _service.UpdateAsync(1, null, null, null);

        
        Assert.Equal("Original", result.Nome);
        Assert.Equal("Descricao original", result.Descricao);
        _midiaRepoMock.Verify(r => r.UpdateAsync(midia), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenFileDoesNotExist_StillDeletesMidia()
    {
        
        var midia = new Midia { Id = 1, Nome = "Teste", UrlMidia = "Uploads/Imagens/inexistente.jpg" };
        _midiaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(midia);

        
        if (File.Exists(midia.UrlMidia))
            File.Delete(midia.UrlMidia);

        
        await _service.DeleteAsync(1);

        
        _midiaRepoMock.Verify(r => r.DeleteAsync(midia), Times.Once);
    }

}
