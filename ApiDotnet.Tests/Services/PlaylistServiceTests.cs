using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api_dotnet.Models;
using api_dotnet.Services.Playlists;
using api_dotnet.Repositories.Interfaces;
using AutoMapper;
using api_dotnet.Models.Dtos;

public class PlaylistServiceTests
{
    private readonly Mock<IPlaylistRepository> _playlistRepoMock;
    private readonly IMapper _mapper;
    private readonly PlaylistService _service;

    public PlaylistServiceTests()
    {
        _playlistRepoMock = new Mock<IPlaylistRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Playlist, PlaylistDto>();
            cfg.CreateMap<Playlist, PlaylistNomeDto>();
        });
        _mapper = config.CreateMapper();

        _service = new PlaylistService(_playlistRepoMock.Object, _mapper);
    }

    // -----------------------------
    // GetAllAsync
    // -----------------------------
    [Fact]
    public async Task GetAllAsync_ReturnsPlaylists()
    {
        // Arrange
        var playlists = new List<Playlist> { new Playlist { Id = 1, Nome = "Teste" } };
        _playlistRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(playlists);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("Teste", ((List<PlaylistDto>)result)[0].Nome);
    }

    // -----------------------------
    // GetByIdAsync
    // -----------------------------
    [Fact]
    public async Task GetByIdAsync_WhenExists_ReturnsPlaylist()
    {
        // Arrange
        var playlist = new Playlist { Id = 1, Nome = "Favoritas" };
        _playlistRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(playlist);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Equal("Favoritas", result.Nome);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        _playlistRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                         .ReturnsAsync((Playlist)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(1));
    }

    // -----------------------------
    // UpdateAsync
    // -----------------------------
    [Fact]
    public async Task UpdateAsync_WhenExists_UpdatesNome()
    {
        // Arrange
        var playlist = new Playlist { Id = 1, Nome = "Antigo" };
        _playlistRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(playlist);

        // Act
        var result = await _service.UpdateAsync(1, "Novo");

        // Assert
        Assert.Equal("Novo", result.Nome);
        _playlistRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        _playlistRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                         .ReturnsAsync((Playlist)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(1, "Novo"));
    }

    // -----------------------------
    // CreateAsync
    // -----------------------------
    [Fact]
    public async Task CreateAsync_CreatesPlaylist()
    {
        // Arrange
        var playlist = new Playlist { Id = 1, Nome = "Nova" };
        _playlistRepoMock.Setup(r => r.CreateAsync(It.IsAny<Playlist>()))
                         .ReturnsAsync(playlist);

        // Act
        var result = await _service.CreateAsync(playlist);

        // Assert
        Assert.Equal("Nova", result.Nome);
    }

    // -----------------------------
    // DeleteAsync
    // -----------------------------
    [Fact]
    public async Task DeleteAsync_WhenExists_DeletesPlaylist()
    {
        // Arrange
        var playlist = new Playlist { Id = 1, Nome = "Apagar" };
        _playlistRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(playlist);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _playlistRepoMock.Verify(r => r.DeleteAsync(playlist), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        _playlistRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                         .ReturnsAsync((Playlist)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(1));
    }

    // -----------------------------
    // AddMidiaAsync
    // -----------------------------
    [Fact]
    public async Task AddMidiaAsync_WhenNew_AddsMidia()
    {
        // Arrange
        _playlistRepoMock.Setup(r => r.GetMidiaPlaylistAsync(1, 10))
                         .ReturnsAsync((MidiaPlaylist)null);

        // Act
        await _service.AddMidiaAsync(1, 10, true);

        // Assert
        _playlistRepoMock.Verify(r => r.AddMidiaPlaylistAsync(It.IsAny<MidiaPlaylist>()), Times.Once);
    }

    [Fact]
    public async Task AddMidiaAsync_WhenAlreadyExists_ThrowsInvalidOperationException()
    {
        // Arrange
        var midiaPlaylist = new MidiaPlaylist { PlaylistId = 1, MidiaId = 10 };
        _playlistRepoMock.Setup(r => r.GetMidiaPlaylistAsync(1, 10))
                         .ReturnsAsync(midiaPlaylist);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddMidiaAsync(1, 10));
    }

    // -----------------------------
    // RemoveMidiaAsync
    // -----------------------------
    [Fact]
    public async Task RemoveMidiaAsync_WhenExists_RemovesMidia()
    {
        // Arrange
        var midiaPlaylist = new MidiaPlaylist { PlaylistId = 1, MidiaId = 10 };
        _playlistRepoMock.Setup(r => r.GetMidiaPlaylistAsync(1, 10))
                         .ReturnsAsync(midiaPlaylist);

        // Act
        await _service.RemoveMidiaAsync(1, 10);

        // Assert
        _playlistRepoMock.Verify(r => r.RemoveMidiaPlaylistAsync(midiaPlaylist), Times.Once);
    }

    [Fact]
    public async Task RemoveMidiaAsync_WhenNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        _playlistRepoMock.Setup(r => r.GetMidiaPlaylistAsync(1, 10))
                         .ReturnsAsync((MidiaPlaylist)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.RemoveMidiaAsync(1, 10));
    }

    // -----------------------------
    // UpdateExibirNoPlayer
    // -----------------------------
    [Fact]
    public async Task UpdateExibirNoPlayer_WhenExists_UpdatesFlag()
    {
        // Arrange
        var midiaPlaylist = new MidiaPlaylist { PlaylistId = 1, MidiaId = 10, ExibirNoPlayer = false };
        _playlistRepoMock.Setup(r => r.GetMidiaPlaylistAsync(1, 10))
                         .ReturnsAsync(midiaPlaylist);

        // Act
        await _service.UpdateExibirNoPlayer(1, 10, true);

        // Assert
        Assert.True(midiaPlaylist.ExibirNoPlayer);
        _playlistRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateExibirNoPlayer_WhenNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        _playlistRepoMock.Setup(r => r.GetMidiaPlaylistAsync(It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync((MidiaPlaylist)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateExibirNoPlayer(1, 10, true));
    }
}
