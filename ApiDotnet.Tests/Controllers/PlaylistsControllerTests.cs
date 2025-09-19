using Xunit;
using Moq;
using api_dotnet.Controllers;
using api_dotnet.Services.Playlists;
using api_dotnet.Models;
using api_dotnet.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class PlaylistsControllerTests
{
    private readonly Mock<IPlaylistService> _playlistServiceMock;
    private readonly PlaylistsController _controller;

    public PlaylistsControllerTests()
    {
        _playlistServiceMock = new Mock<IPlaylistService>();
        _controller = new PlaylistsController(_playlistServiceMock.Object);
    }

    [Fact]
    public async Task Get_ReturnsOkWithPlaylists()
    {
        _playlistServiceMock.Setup(s => s.GetAllAsync())
            .ReturnsAsync(new List<PlaylistDto> { new PlaylistDto { Id = 1, Nome = "Favoritas" } });

        var result = await _controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var playlists = Assert.IsAssignableFrom<IEnumerable<PlaylistDto>>(okResult.Value);
        Assert.Single(playlists);
    }

    [Fact]
    public async Task GetById_WhenExists_ReturnsOk()
    {
        var dto = new PlaylistDto { Id = 1, Nome = "Favoritas" };
        _playlistServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

        var result = await _controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var playlist = Assert.IsType<PlaylistDto>(okResult.Value);
        Assert.Equal("Favoritas", playlist.Nome);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ReturnsNotFound()
    {
        _playlistServiceMock.Setup(s => s.GetByIdAsync(1)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.GetById(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var playlist = new Playlist { Id = 1, Nome = "Nova" };
        var dto = new PlaylistDto { Id = 1, Nome = "Nova" };
        _playlistServiceMock.Setup(s => s.CreateAsync(playlist)).ReturnsAsync(dto);

        var result = await _controller.Create(playlist);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("Nova", ((PlaylistDto)createdResult.Value).Nome);
    }

    [Fact]
    public async Task Update_WhenSuccessful_ReturnsOk()
    {
        var updatedDto = new PlaylistNomeDto { Nome = "Atualizada" };
        _playlistServiceMock.Setup(s => s.UpdateAsync(1, "Atualizada")).ReturnsAsync(updatedDto);

        var result = await _controller.Update(1, new PlaylistNomeDto { Nome = "Atualizada" });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<PlaylistNomeDto>(okResult.Value);
        Assert.Equal("Atualizada", dto.Nome);
    }

    [Fact]
    public async Task Update_WhenNotFound_ReturnsNotFound()
    {
        _playlistServiceMock.Setup(s => s.UpdateAsync(1, "Novo")).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.Update(1, new PlaylistNomeDto { Nome = "Novo" });

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Delete_WhenSuccessful_ReturnsNoContent()
    {
        _playlistServiceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_WhenNotFound_ReturnsNotFound()
    {
        _playlistServiceMock.Setup(s => s.DeleteAsync(1)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.Delete(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task AddMidia_WhenSuccessful_ReturnsNoContent()
    {
        _playlistServiceMock.Setup(s => s.AddMidiaAsync(1, 2, true)).Returns(Task.CompletedTask);

        var result = await _controller.AddMidia(1, 2);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task AddMidia_WhenConflict_ReturnsConflict()
    {
        _playlistServiceMock.Setup(s => s.AddMidiaAsync(1, 2, true)).ThrowsAsync(new InvalidOperationException());

        var result = await _controller.AddMidia(1, 2);

        var conflict = Assert.IsType<ConflictObjectResult>(result);
        Assert.Contains("Mídia já associada", conflict.Value.ToString());
    }

    [Fact]
    public async Task UpdateExibirNoPlayer_WhenSuccessful_ReturnsNoContent()
    {
        _playlistServiceMock.Setup(s => s.UpdateExibirNoPlayer(1, 2, true)).Returns(Task.CompletedTask);

        var result = await _controller.UpdateExibirNoPlayer(1, 2, true);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateExibirNoPlayer_WhenNotFound_ReturnsNotFound()
    {
        _playlistServiceMock.Setup(s => s.UpdateExibirNoPlayer(1, 2, true)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.UpdateExibirNoPlayer(1, 2, true);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task RemoveMidia_WhenSuccessful_ReturnsNoContent()
    {
        _playlistServiceMock.Setup(s => s.RemoveMidiaAsync(1, 2)).Returns(Task.CompletedTask);

        var result = await _controller.RemoveMidia(1, 2);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task RemoveMidia_WhenNotFound_ReturnsNotFound()
    {
        _playlistServiceMock.Setup(s => s.RemoveMidiaAsync(1, 2)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.RemoveMidia(1, 2);

        Assert.IsType<NotFoundResult>(result);
    }
}
